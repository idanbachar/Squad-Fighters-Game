using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Linq;

namespace SquadFighters
{
    public class SquadFighters : Game
    {
        public static GraphicsDeviceManager Graphics;
        private SpriteBatch spriteBatch; //ציור

        public Player Player; //שחקן נוכחי
        private Camera Camera; //מצלמה
        private MainMenu MainMenu; //תפריט ראשי
        private GameState GameState; //סוג משחק
        private Map Map; //מפה
        private HUD HUD; //UI
        private TcpClient Client; // קליינט נוכחי
        private Dictionary<string, Player> Players; //שחקנים שהתחברו
        public static ContentManager ContentManager; //טעינה
        private bool isPressed; //האם הייתה נקישה

        private Vector2 CameraPosition; //מיקום התבייתות מצלמה
        private int CameraPlayersIndex; //אינדקס מצלמה

        //משתני רשת
        private Thread ReceiveThread; //קבלת נתונים מהשרת
        private Thread SendPlayerDataThread; //שליחת נתונים לשרת
        private string ServerIp; // כתובת אייפי של השרת
        private int ServerPort; //כתובת פורט של השרת
        public string[] ReceivedDataArray; // מערך נתונים שהתקבלו
        private int MaxItems; //כמות מקסימלית של פריטים שאמורים להטען

        public SquadFighters()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 450;
            Graphics.PreferredBackBufferHeight = 650;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Players = new Dictionary<string, Player>();
            ServerIp = "192.168.1.17";
            ServerPort = 7895;
            Window.Title = "SquadFighters: Battle Royale";
            GameState = GameState.MainMenu;
            CameraPlayersIndex = -1;
        }

        protected override void Initialize()
        {
            base.Initialize();
            ContentManager = Content;
        }

        //חיבור לשרת של המשחק
        public void ConnectToServer(string serverIp, int serverPort)
        {
            // נסה להתחבר לשרת
            try
            {
                Client = new TcpClient(ServerIp, ServerPort); //ניסיון התחברות לשרת
                SendOneDataToServer("Load Map"); //במידה והצליח שלח לשרת הודעת טעינת מפה
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //הוספת שחקן נוסף שהתחבר לשרת
        public void AddPlayer(string CurrentConnectedPlayerName)
        {
            Player player = new Player(CurrentConnectedPlayerName);
            player.LoadContent(Content);
            Players.Add(CurrentConnectedPlayerName, player);

            PlayerCard playerCard = new PlayerCard(player.Name, player.Health, player.BulletsCapacity + "/" + player.MaxBulletsCapacity);
            playerCard.LoadContent(Content);

            HUD.PlayersCards.Add(playerCard);
        }

        //הצטרפות למשחק, התחלת קבלת נתונים מהשרת
        public void JoinGame()
        {
            Player = new Player("idan" + new Random().Next(1000));
            Player.LoadContent(Content);

            HUD.PlayerCard = new PlayerCard(Player.Name, Player.Health, Player.BulletsCapacity + "/" + Player.MaxBulletsCapacity);
            HUD.PlayerCard.Visible = true;
            HUD.PlayerCard.LoadContent(Content);
 
            ConnectToServer(ServerIp, ServerPort);

            ReceiveThread = new Thread(ReceiveDataFromServer);
            ReceiveThread.Start();
        }

        //שליחת מידע בודד לשרת
        public void SendOneDataToServer(string data)
        {
            try
            {
                NetworkStream stream = Client.GetStream();
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();

                Thread.Sleep(30);
            }
            catch (Exception)
            {

            }
        }

        //שליחת נתוני השחקן הנוכחי לשרת
        public void SendPlayerDataToServer()
        {
            while (true)
            {
                string data = Player.ToString();
                try
                {
                    NetworkStream stream = Client.GetStream();
                    byte[] bytes = Encoding.ASCII.GetBytes(data);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();


                    Thread.Sleep(200);
                }
                catch (Exception)
                {

                }
            }
        }
        
        //התנתקות מהשרת
        public void DisconnectFromServer()
        {
            try
            {
                ReceiveThread.Abort();
                SendPlayerDataThread.Abort();
                Client.Close();
            }
            catch (Exception)
            {

            }

        }

        //קבלת נתונים מהשרת
        public void ReceiveDataFromServer()
        {
            //רוץ כל הזמן
            while (true)
            {
                //נסה לבצע
                try
                {
                    NetworkStream netStream = Client.GetStream();
                    byte[] bytes = new byte[10024];
                    netStream.Read(bytes, 0, bytes.Length);
                    string data = Encoding.ASCII.GetString(bytes);
                    string ReceivedDataString = data.Substring(0, data.IndexOf("\0"));
                    ReceivedDataArray = ReceivedDataString.Split(','); //מערך שבו כל הפרמטרים מופרדים באמצעות פסיקים

                    //בדיקה אם התחבר שחקן
                    if (ReceivedDataString.Contains("Connected"))
                    {
                        string CurrentConnectedPlayerName = ReceivedDataString.Split(',')[0];

                        if (CurrentConnectedPlayerName != Player.Name)
                        {
                            AddPlayer(CurrentConnectedPlayerName);
                            Console.WriteLine(ReceivedDataString);
                        }
                    } //בדיקה אם במידע שהתקבל מופיע שם השחקן, ובמידה וכן עדכן את הנתונים שלו
                    else if (ReceivedDataString.Contains("PlayerName"))
                    {
                        string playerName = ReceivedDataArray[0].Split('=')[1];
                        float playerX = float.Parse(ReceivedDataArray[1].Split('=')[1]);
                        float playerY = float.Parse(ReceivedDataArray[2].Split('=')[1]);
                        float playerRotation = float.Parse(ReceivedDataArray[3].Split('=')[1]);
                        int playerHealth = int.Parse(ReceivedDataArray[4].Split('=')[1]);
                        bool playerIsShoot = bool.Parse(ReceivedDataArray[5].Split('=')[1]);
                        float playerDirectionX = float.Parse(ReceivedDataArray[6].Split('=')[1]);
                        float playerDirectionY = float.Parse(ReceivedDataArray[7].Split('=')[1]);
                        bool playerIsSwimming = bool.Parse(ReceivedDataArray[8].Split('=')[1]);
                        bool playerIsShield = bool.Parse(ReceivedDataArray[9].Split('=')[1]);
                        ShieldType playerShieldType = (ShieldType)int.Parse(ReceivedDataArray[10].Split('=')[1]);
                        int playerBulletsCapacity = int.Parse(ReceivedDataArray[11].Split('=')[1]);
                        bool playerIsDead = bool.Parse(ReceivedDataArray[12].Split('=')[1]);

                        //כל זה יקרה אך ורק אם השחקן אכן התחבר מקודם
                        if (Players.ContainsKey(playerName))
                        {
                            Players[playerName].Name = playerName; //שם שחקן
                            Players[playerName].Position.X = playerX; //מיקום קורדינטת X של השחקן   
                            Players[playerName].Position.Y = playerY; // מיקום קורדינטת Y של השחקן
                            Players[playerName].Rotation = playerRotation; //זווית השחקן
                            Players[playerName].Health = playerHealth; //בריאות השחקן
                            Players[playerName].IsShoot = playerIsShoot; //האם השחקן יורה
                            Players[playerName].Direction.X = playerDirectionX; //כיוון השחקן בציר ה X
                            Players[playerName].Direction.Y = playerDirectionY; // כיוון השחקן בציר ה Y
                            Players[playerName].IsSwimming = playerIsSwimming; // האם השחקן שוחה
                            Players[playerName].IsShield = playerIsShield; // האם לשחקן יש הגנה
                            Players[playerName].ShieldType = playerShieldType; // סוג ההגנה
                            Players[playerName].BulletsCapacity = playerBulletsCapacity; //כמות תחמושת נוכחית
                            Players[playerName].IsDead = playerIsDead; // האם השחקן מת
                            
                            //עדכון הערכים עבור אותו שחקן בכרטיסייה שלו
                            for (int i = 0; i < HUD.PlayersCards.Count; i++)
                            {
                                //בדוק אם שם בעל הכרטיסייה שווה לשם השחקן שהגיע עליו המידע
                                if (HUD.PlayersCards[i].PlayerName == playerName) 
                                {
                                    // עדכן האם יכול להציג את הבועות ליד הכרטיסייה של השחקן שהתקבל
                                    HUD.PlayersCards[i].CanBubble = playerIsSwimming;

                                    //עשה פעולה זו רק אם אין לך הגנה מלפני
                                    if ((HUD.PlayersCards[i].ShieldBars[0].ShieldType != playerShieldType))
                                    {
                                        //עדכון בר ההגנה בכרטיסיה של אותו שחקן
                                        for (int j = 0; j < HUD.PlayersCards[i].ShieldBars.Length; j++) //רוץ על כל ברי ההגנה
                                        {
                                            HUD.PlayersCards[i].ShieldBars[j].ShieldType = Players[playerName].ShieldType; //עדכן את סוג בר ההגנה בכרטיסייה
                                            HUD.PlayersCards[i].ShieldBars[j].LoadContent(Content); // טען את סוג בר ההגנה בכרטיסייה
                                        }
                                    }
                                }
                            }
                        } 
                        else //במידה ולא התחבר מקודם השחקן, וזו הפעם הראשונה
                        {
                            //צור שחקן חדש
                            AddPlayer(playerName);
                        }

                    } //במידה והמידע שהתקבל מכיל בתוכו הוספת פריט
                    else if (ReceivedDataString.Contains("AddItem=true"))
                    {
                        ItemCategory ItemCategory = (ItemCategory)int.Parse(ReceivedDataArray[1].Split('=')[1]); //קטגוריית פריט
                        int type = int.Parse(ReceivedDataArray[2].Split('=')[1].ToString()); // סוג פריט
                        float itemX = float.Parse(ReceivedDataArray[3].Split('=')[1].ToString()); //קורדינטת X של הפריט
                        float itemY = float.Parse(ReceivedDataArray[4].Split('=')[1].ToString()); // קורדינטת Y של הפריט
                        int itemCapacity = int.Parse(ReceivedDataArray[5].Split('=')[1].ToString()); // כמות פריט
                        string itemKey = ReceivedDataArray[6].Split('=')[1].ToString(); //מפתח מילון של הפריט
                        MaxItems = int.Parse(ReceivedDataArray[7].Split('=')[1].ToString()); //כמות מקסימלית של פריטים

                        //הכנס למפה את הפריטים שהתקבלו
                        Map.AddItem(ItemCategory, type, itemX, itemY, itemCapacity, itemKey);

                        //הדפס בקונסול
                        Console.WriteLine(ReceivedDataString);


                    } //במידה והמידע שהתקבל מכיל מחיקת פריט
                    else if (ReceivedDataString.Contains("Remove Item"))
                    {
                        string itemKey = ReceivedDataArray[1];
                        Map.Items.Remove(itemKey); //הסר את הפריט שהתקבל
                        Console.WriteLine(ReceivedDataString);
                    }//במידע והמידע שהתקבל מכיל עדכון כמות פריט
                    else if (ReceivedDataString.Contains("Update Item Capacity"))
                    {
                        string itemKey = ReceivedDataArray[2];
                        int receivedCapacity = int.Parse(ReceivedDataArray[1]);

                        //בודק אם העדכון של כמות הפריט הוא מסוג כדורים לרובה
                        if (Map.Items[itemKey] is GunAmmo)
                        {
                            //אם כן עדכן במפה את הכמות של הפריט שהתקבל
                            ((GunAmmo)(Map.Items[itemKey])).Capacity = receivedCapacity;
                        }

                        //הדפס בקונסול
                        Console.WriteLine(ReceivedDataString);

                    } //במידה והמידע שהתקבל מכיל טעינת פריטים הסתיימה
                    else if (ReceivedDataString.Contains("Load Items Completed"))
                    {
                        //מצב משחק יתחלף למשחק
                        GameState = GameState.Game;

                        //שלח הודעה לשרת שהצטרף שחקן
                        SendOneDataToServer(Player.Name + ",Connected");

                        Thread.Sleep(500);

                        //והתחל לשלוח באופן חוזר מידע על השחקן הנוכחי
                        SendPlayerDataThread = new Thread(() => SendPlayerDataToServer());
                        SendPlayerDataThread.Start();

                        Console.WriteLine(ReceivedDataString);
                    }
                    //במידה והתקבל מידע על ירייה
                    else if (ReceivedDataString.Contains("ShootData"))
                    {
                        string playerName =  ReceivedDataArray[1].Split('=')[1];

                        //בצע ירייה עבור השחקן שירה
                        Players[playerName].Shoot();
                    }

                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        //בדיקת מגע עם הפריטים שבמפה
        public void CheckItemsIntersects(Dictionary<string, Item> items)
        {
            //נסה לבצע
            try
            {
                //רוץ על הפריטים שבמפה
                for (int i = 0; i < items.Count; i++)
                {
                    //אם השחקן נוגע באחד הפריטים
                    if (Player.Rectangle.Intersects(items.ElementAt(i).Value.Rectangle))
                    {
                        //בדוק אם זה אוכל
                        if (items.ElementAt(i).Value is Food)
                        {
                            //אם הבריאות של השחקן לא 100%
                            if (Player.Health < 100)
                            {
                                int heal = ((Food)(items.ElementAt(i).Value)).GetHealth(); //השג את כמות הבריאות שהאוכל שנגע בשחקן מעניק
                                Player.Heal(heal); //העלאת בריאות לשחקן
                                string key = items.ElementAt(i).Key; //השגת המפתח של המילון

                                items.Remove(key); //מחק את הפריט מהמפה
                                SendOneDataToServer("Remove Item," + key); //שלח עדכון לשרת על הפריט שנמחק על מנת שיימחק גם בשרת
                            }
                        } //אם זה תחמושת
                        else if (items.ElementAt(i).Value is GunAmmo)
                        {
                            int capacity = ((GunAmmo)(items.ElementAt(i).Value)).Capacity; //השג את כמות התחמושת שנגע בה השחקן

                            if (Player.BulletsCapacity + capacity <= Player.MaxBulletsCapacity) //בדיקה שכמות התחמושת הנוכחית של השחקן בשילוב של התחמושת שקיבל קטנה מהכמות המקסימלית שיכול להחזיק
                            {
                                Player.BulletsCapacity += capacity; //עדכן את תחמושת השחקן בתחמושת החדשה
                                string key = items.ElementAt(i).Key; // השגת המפתח של המילון

                                items.Remove(key); //מחק את הפריט מהמפה
                                SendOneDataToServer("Remove Item," + key); //שלח עדכון לשרת על הפריט שנמחק על מנת שיימחק גם בשרת
                            }
                            else //במידה והתחמושת שנגע בה השחקן תביא לשחקן יותר תחמושת משיכול לסחוב
                            {
                                //בדוק אם התחמושת של השחקן בשילוב עם התחמושת שקיבל גדולה מהכמות המקסימלית שיכול להחזיק וגם שכמות הכדורים הנוכחית לא שווה לכמות המקסימלית
                                if (Player.BulletsCapacity + capacity > Player.MaxBulletsCapacity && Player.BulletsCapacity != Player.MaxBulletsCapacity)
                                {
                                    ((GunAmmo)(items.ElementAt(i).Value)).Capacity -= Player.MaxBulletsCapacity - Player.BulletsCapacity; //עדכן בפריט את עודף התחמושת שנשאר
                                    Player.BulletsCapacity = Player.MaxBulletsCapacity; //מלא את כמות התחמושת של השחקן עד הכמות המקסימלית

                                    int itemCapacity = ((GunAmmo)(items.ElementAt(i).Value)).Capacity; //השג את כמות התחמושת שנשארה לפריט לאחר השינוי
                                    string key = items.ElementAt(i).Key; //השג את המפתח של המילון
                                    SendOneDataToServer("Update Item Capacity," + itemCapacity + "," + key); //שלח עדכון לשרת על עדכון הכמות של הפריט
                                }
                            }
                        } // אם זה מגן
                        else if (items.ElementAt(i).Value is Shield)
                        {
                            //אם סוג ההגנה של השחקן היא לא ללא הגנה או שיש לשחקן הגנה והיא נמוכה מההגנה של המגן הנוכחי
                            if (Player.ShieldType == ShieldType.None || Player.ShieldType < ((Shield)items.ElementAt(i).Value).ItemType)
                            {
                                Player.ShieldType = ((Shield)items.ElementAt(i).Value).ItemType; //השג את סוג המגן

                                //רוץ על כל ברי ההגנה שבכרטיסיית השחקן הנוכחי
                                for (int j = 0; j < HUD.PlayerCard.ShieldBars.Length; j++)
                                {
                                    HUD.PlayerCard.ShieldBars[j].ShieldType = ((Shield)items.ElementAt(i).Value).ItemType; //עדכן את סוג ההגנה
                                    HUD.PlayerCard.ShieldBars[j].LoadContent(Content); //טען את ברי ההגנה
                                }
                                Player.IsShield = true; //עדכן את השחקן למכיל הגנה

                                string key = items.ElementAt(i).Key; //השגת מפתח המילון
                                items.Remove(key); //מחיקת הפריט
                                SendOneDataToServer("Remove Item," + key); //שלח עדכון לשרת על הפריט שנמחק על מנת שיימחק גם בשרת
                            }

                        } //לא בשימוש
                        else if (items.ElementAt(i).Value is Helmet)
                        {
                            string key = items.ElementAt(i).Key;
                            items.Remove(key);
                            SendOneDataToServer("Remove Item," + key);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        // טעינת המשחק
        protected override void LoadContent()
        {
            // יצירת מחלקת הציור
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //יצירת מחלקת המצלמה
            Camera = new Camera(GraphicsDevice.Viewport);

            //יצירת מערכת התצוגה UI של השחקן
            HUD = new HUD();
            HUD.LoadContent(Content);

            //הגדרת משתנה אקראי
            Random rndItem = new Random();

            //יצירת התפריט הראשי
            MainMenu = new MainMenu(2);
            MainMenu.LoadContent(Content);

            //יצירת המפה
            Map = new Map(new Rectangle(0, 0, 5000, 5000));
            Map.LoadContent(Content);
        }

        protected override void UnloadContent()
        {

        }

        //קבלת אובייקט של שחקן על ידי שם בלבד
        public Player GetPlayerByName(string name)
        {
            // בודק אם שם השחקן שהתקבל מופיע במילון
            foreach (KeyValuePair<string, Player> otherPlayer in Players)
                if (name == otherPlayer.Key) return otherPlayer.Value; //אם כן תחזיר את השחקן

            //במידה ולא מצא את שם השחקן שהתקבל במילון, בדוק אם הוא שווה לשם השחקן הנוכחי
            if (name == Player.Name)
                return Player; //אם כן תחזיר את השחקן הנוכחי

            return null; // אחרת תחזיר אובייקט ריק
        }

        //עדכון משחק
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                DisconnectFromServer();
                Exit();
            }

            // אם סוג המשחק הוא משחק
            if (GameState == GameState.Game)
            {
                // אם השחקן הנוכחי
                if (Player.IsDead)
                {
                    // אם נלחץ חץ ימיני במסך
                    if (Keyboard.GetState().IsKeyDown(Keys.Right) && !isPressed)
                    {
                        isPressed = true;

                        //דפדף במצלמה במצב של spectate על שחקנים אחרים
                        if (CameraPlayersIndex < Players.Count - 1)
                        {
                            CameraPlayersIndex++;
                            CameraPosition = Players.ElementAt(CameraPlayersIndex).Value.Position;
                        }
                        else
                        {
                            CameraPlayersIndex = -1;
                            CameraPosition = Player.Position;
                        }
                    }

                    if (Keyboard.GetState().IsKeyUp(Keys.Right))
                    {
                        isPressed = false;
                    }
                }
                else //אם השחקן הנוכחי לא מת
                {
                    // המצלמה תתביית על השחקן הנוכחי בלבד
                    CameraPosition = Player.Position;
                }

                // אם השחקן לא מת
                if (!Player.IsDead)
                    Camera.Focus(Player.Position, Map.Rectangle.Width, Map.Rectangle.Height); // המצלמה תתביית על השחקן הנוכחי בלבד
                else if(Player.IsDead && CameraPlayersIndex != -1)  // המצלמה תיתן להחליף בין מצלמת שחקנים spectate
                    Camera.Focus(Players.ElementAt(CameraPlayersIndex).Value.Position, Map.Rectangle.Width, Map.Rectangle.Height);
                else if(Player.IsDead && CameraPlayersIndex == -1) //המצלמה תתביית על השחקן הנוכחי בלבד
                    Camera.Focus(Player.Position, Map.Rectangle.Width, Map.Rectangle.Height);

                //אם השחקן הנוכחי הקיש tab
                if (Keyboard.GetState().IsKeyDown(Keys.Tab))
                {
                    //השחקן יראה את הכרטיסיות של השחקנים האחרים
                    foreach (PlayerCard playerCard in HUD.PlayersCards)
                        playerCard.Visible = true;
                }
                // אחרת
                if (Keyboard.GetState().IsKeyUp(Keys.Tab))
                {
                    // השחקן לא יראה את הכרטיסיות של השחקנים האחרים
                    foreach (PlayerCard playerCard in HUD.PlayersCards)
                        playerCard.Visible = false;
                }

                // אם השחקן הנוכחי הקיש רווח
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !Player.IsShoot)
                {

                    // אם לשחקן הנוכחי יש תחמושת
                    if (Player.BulletsCapacity > 0)
                    {
                        // השחקן הנוכחי יבצע יריה
                        Player.Shoot();

                        //וישלח עדכון לשרת שהוא ירה
                        SendOneDataToServer("ShootData=true,PlayerShotName=" + Player.Name);
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.Space))
                {
                    Player.IsShoot = false;
                }

                //עדכון תמידי של השחקן הנוכחי
                Player.Update(Map);

                //עדכון תמידי ובדיקת נגיעת השחקן הנוכחי בשאר הפריטים שבמשחק
                CheckItemsIntersects(Map.Items);

                //עדכון תמידי של כרטיסיית השחקן הנוכחי
                HUD.PlayerCard.Update(Player, new Vector2(0, 0));

                //אם השחקן הנוכחי סיים את כל הבועות כשהוא בתוך המים
                if (HUD.PlayerCard.IsBubbleHit)
                {
                    //השחקן ייפגע
                    Player.Hit(1);
                }

                // רוץ על כל כרטיסיות השחקנים שהתחברו
                for (int i = 0; i < HUD.PlayersCards.Count; i++)
                {
                    string playerName = HUD.PlayersCards[i].PlayerName; // השג את שם השחקנים

                    // מקם את מיקום הכרטיסייה שלהם אחד מעל השני
                    Vector2 position = new Vector2(HUD.PlayersCards[i].CardPosition.X, HUD.PlayerCard.CardRectangle.Height + 10 + HUD.PlayersCards[i].CardRectangle.Height * i);

                    // אדכן את פרטי כרטיסיות השחקנים לפי המידע שהתקבל על השחקנים
                    HUD.PlayersCards[i].Update(GetPlayerByName(playerName), position);
                }

                // רןץ על כל השחקנים שהתחברו
                foreach (KeyValuePair<string, Player> otherPlayer in Players)
                {
                    // עדכן את המלבן שמקיף אותם
                    otherPlayer.Value.UpdateRectangle();

                    // רוץ גם על כל היריות שהם ירו
                    for (int i = 0; i < otherPlayer.Value.Bullets.Count; i++)
                    {
                        // אם היריה שלהם פגעה בשחקן הנוכחי
                        if (otherPlayer.Value.Bullets[i].Rectangle.Intersects(Player.Rectangle))
                        {
                            //הפסק את היריה
                            otherPlayer.Value.Bullets[i].IsFinished = true;

                            ShieldType playerShieldType = HUD.PlayerCard.ShieldBars[2].ShieldType; // השג את סוג הבר של השחקן הנוכחי

                            // בדוק את סוג ההגנה של השחקן
                            switch (playerShieldType)
                            {
                                case ShieldType.None: // במידה ואין
                                    Player.Hit(otherPlayer.Value.Bullets[i].Damage); // תוריד לשחקן הנוכחי את הבריאות לפי עוצמת פגיעת היריה
                                    break;
                                case ShieldType.Shield_Level_1: // אם לשחקן יש הגנה בכללי
                                case ShieldType.Shield_Level_2:
                                case ShieldType.Shield_Rare:
                                case ShieldType.Shield_Legendery:

                                    //רוץ על כל ברי ההגנה שבכל הכרטיסיות של השחקן הנוכחי
                                    for (int j = 0; j < HUD.PlayerCard.ShieldBars.Length; j++)
                                    {
                                        // הורד הגנה בהתאם
                                        if (HUD.PlayerCard.ShieldBars[0].Armor > 0)
                                            HUD.PlayerCard.ShieldBars[0].Hit(otherPlayer.Value.Bullets[i].Damage);
                                        else
                                        {
                                            if (HUD.PlayerCard.ShieldBars[1].Armor > 0)
                                                HUD.PlayerCard.ShieldBars[1].Hit(otherPlayer.Value.Bullets[i].Damage);
                                            else
                                            {
                                                if (HUD.PlayerCard.ShieldBars[2].Armor > 0)
                                                    HUD.PlayerCard.ShieldBars[2].Hit(otherPlayer.Value.Bullets[i].Damage);
                                            }
                                        }

                                    }
                                    break;
                            }
                            
                        }

                        // במידה ולא נעצרה הירייה
                        if (!otherPlayer.Value.Bullets[i].IsFinished)
                            otherPlayer.Value.Bullets[i].Update(); //הזז את הירייה
                        else // במידה ונעצרה הירייה
                            otherPlayer.Value.Bullets.RemoveAt(i); //מחק את הירייה
                    }
                }

                // רוץ על כל היריות של השחקן הנוכחי
                for (int i = 0; i < Player.Bullets.Count; i++)
                {
                    // אם היריה לא נעצרה
                    if (!Player.Bullets[i].IsFinished)
                    {
                        // הזז את הירייה
                        Player.Bullets[i].Update();

                        // רוץ על כל השחקנים שהתחברו
                        for (int j = 0; j < Players.Count; j++)
                        {
                            // אם הכדור נגע באחד השחקנים שהתחברו
                            if (Player.Bullets[i].Rectangle.Intersects(Players.ElementAt(j).Value.Rectangle))
                                Player.Bullets[i].IsFinished = true; // עצור את הירייה

                        }
                    }
                    else // אחרת
                        Player.Bullets.RemoveAt(i); //מחק את הירייה
                }

                // נסה
                try
                {
                    // רוץ על כל הפריטים שבמפה
                    for (int i = 0; i < Map.Items.Count; i++)
                        Map.Items.ElementAt(i).Value.Update(); //עדכן את הפריטים שבמפה
                }
                catch (Exception)
                {

                }
            } // במידה וסוג המשחק הוא תפריט ראשי
            else if(GameState == GameState.MainMenu)
            {
                //במידה ונלחץ קליק שמאלי בעכבר
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !isPressed)
                {
                    isPressed = true;

                    //ונהיה על הכפתור של Join Game
                    if (MainMenu.Buttons[0].Rectangle.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 16, 16)))
                    {
                        // תתחיל טעינת הפריטים במפה
                        GameState = GameState.Loading;
                        JoinGame();
                    }

                    //ונהיה על Exit
                    if (MainMenu.Buttons[1].Rectangle.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 16, 16)))
                    {
                        // צא מהמשחק
                        Exit();
                    }
                }

                if(Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    isPressed = false;
                }
            }

            base.Update(gameTime);
        }

        //ציור המשחק
        protected override void Draw(GameTime gameTime)
        {
            // צבע רקע ירוק בהיר
            GraphicsDevice.Clear(Color.LightGreen);
            
            //במידה וסוג המשחק הוא משחק
            if (GameState == GameState.Game)
            {
                //הפעל ציור AlphaBlend שיודע לעבוד עם מצלמה
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);

                //צייר את המפה
                Map.Draw(spriteBatch);

                //נסה
                try
                {
                    //לרוץ על כל הפריטים במפה
                    for (int i = 0; i < Map.Items.Count; i++)
                    {
                        //צייר את כל הפריטים במפה
                        Map.Items.ElementAt(i).Value.Draw(spriteBatch);

                        //צייר את הכמות עבור כל פריט
                        spriteBatch.DrawString(HUD.ItemsCapacityFont, Map.Items.ElementAt(i).Value.ToString(), new Vector2(Map.Items.ElementAt(i).Value.Position.X + 15, Map.Items.ElementAt(i).Value.Position.Y - 30), Color.Black);

                    }
                }
                catch (Exception)
                {

                }


                // רוץ על כל היריות שירה השחקן הנוכחי
                foreach (Bullet bullet in Player.Bullets)
                    bullet.Draw(spriteBatch); //צייר את היריות


                // רוץ על כל השחקנים שהתחברו למשחק
                foreach (KeyValuePair<string, Player> otherPlayer in Players)
                {
                    // אם השחקנים לא בתוך המים
                    if(!otherPlayer.Value.IsSwimming)
                        otherPlayer.Value.Draw(spriteBatch); //צייר את השחקנים

                    // רוץ על כל היריות של השחקנים שהתחברו
                    for (int i = 0; i < otherPlayer.Value.Bullets.Count; i++)
                        otherPlayer.Value.Bullets[i].Draw(spriteBatch); //צייר את היריות של השחקנים שהתחברו

                    // צייר את שמות השחקנים שהתחברו מעל הראש שלהם
                    HUD.DrawPlayersInfo(spriteBatch, otherPlayer.Value);
                }

                // צייר את השחקן הנוכחי
                Player.Draw(spriteBatch);

                // צייר את שם השחקן הנוכחי מעל הראש
                HUD.DrawPlayerInfo(spriteBatch, Player);

                // סיים ציור AlphaBlend
                spriteBatch.End();


                // התחל ציור רגיל UI
                spriteBatch.Begin();

                //צייר UI
                HUD.Draw(spriteBatch, Player, Players);

                // אם לשחקן הנוכחי יש מגן
                if (Player.IsShield)
                {
                    // רוץ על כל ברי ההגנה של השחקן
                    foreach (ShieldBar shieldbar in HUD.PlayerCard.ShieldBars)
                        shieldbar.Draw(spriteBatch); // צייר ברי הגנה
                }

                // סיום ציור רגיל
                spriteBatch.End();
            }
            // במידה וסוג המשחק הוא תפריט
            else if(GameState == GameState.MainMenu)
            {
                //התחל ציור רגיל
                spriteBatch.Begin();

                //צייר Ui את שם המשחק
                HUD.DrawGameTitle(spriteBatch);

                // רוץ על כל הכפתורים שבתפריט
                foreach (Button button in MainMenu.Buttons)
                {
                    // במידה והעכבר נוגע בכפתור
                    if (button.Rectangle.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 16, 16)))
                        button.Draw(spriteBatch, true); // הפוך כפתור לבהיר
                    else//אחרת
                        button.Draw(spriteBatch, false);// הפוך כפתור לכהה
                }

                // סיום ציור רגיל
                spriteBatch.End();
            }
            // במידה וסוג המשחק הוא טעינה
            else if(GameState == GameState.Loading)
            {
                // התחל ציור רגיל
                spriteBatch.Begin();

                // צייר את שם המשחק UI
                HUD.DrawGameTitle(spriteBatch);

                //צייר את כמות אחוזי הטעינה
                HUD.DrawLoading(spriteBatch, MaxItems, Map.Items.Count);


                // סיום ציור רגיל
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
