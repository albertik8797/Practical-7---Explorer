using System.Data;
using System.Transactions;

namespace ConsoleApp1
{
    static class Prov
    {
        public static DriveInfo[] alldrivers = new DriveInfo[10];
        public static string curentpath = "Диски";
        public static string lastpath;
        public static void Create()
        {
            alldrivers = DriveInfo.GetDrives();
        }
        public static string[] folderin(string dir)
        {
            lastpath = curentpath;
            curentpath = dir;
            Directory.SetCurrentDirectory(dir);
            string[] uops = Directory.GetFileSystemEntries(dir);
            //  for (int i=0;i < uops.Count();i++) {uops[i] = uops[i].Substring(dir.Length); }
            return uops;
        }

    }
    public class navicon
    {
        private int startcol = 10, startrow = 5;
        private int newpos, lastpos;
        string[] coun = new string[255];
        string[] couninfo = new string[255];
        public int arrcont = 0;
        private void show()
        {
            Console.SetCursorPosition(startcol - 3, startrow - 3);
            Console.WriteLine(Prov.curentpath);
            for (int i = 0; i < arrcont; i++)
            {
                Console.SetCursorPosition(startcol, startrow + i);
                Console.WriteLine(coun[i]);

                Console.SetCursorPosition(startcol + 50, startrow + i);
                Console.WriteLine(couninfo[i]);
            }
        }
        private void clearshow()
        {
            Console.SetCursorPosition(startcol - 3, startrow - 3);
            Console.WriteLine("                                                    ");

            for (int i = 0; i < arrcont; i++)
            {
                Console.SetCursorPosition(startcol + 50, startrow + i);
                Console.WriteLine("                                                                  ");
            }
        }

        private void showpos()
        {
            Console.SetCursorPosition(startcol - 3, startrow + lastpos);
            Console.Write(' ');

            Console.SetCursorPosition(startcol - 3, startrow + newpos);
            Console.Write('\u0010');

        }
        public void drivetocoun(DriveInfo[] drv)
        {
            arrcont = drv.Count();
            for (int i = 0; i < arrcont; i++)
            {
                coun[i] = drv[i].Name;
                couninfo[i] = drv[i].DriveType.ToString();
            }

            show();
        }
        public void foldertocount(string[] art)
        {
            coun = art;
            arrcont = art.Count();
            for (int i = 0; i < arrcont; i++)
            {
                couninfo[i] = File.GetCreationTime(art[i]).ToString();
            }
            //GetLastAccessTimeUtc(String)
        }
        public int keyinfo()
        {
            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = true;
            int i = 0;
            do
            {
                cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow: i = 1; break;
                    case ConsoleKey.DownArrow: i = 2; break;
                    case ConsoleKey.LeftArrow: i = 3; break;
                    case ConsoleKey.RightArrow: i = 4; break;
                    case ConsoleKey.Escape: i = 5; break;
                    case ConsoleKey.Enter: i = 6; break;
                }
            } while (i == 0);
            return i;
        }
        public void main()
        {
            int i;
            show();
            showpos();
            do
            {
                i = keyinfo();
                if (i != 5) { lastpos = newpos; }
                switch (i)
                {
                    case 1: newpos--; break;
                    case 2: newpos++; break;
                        //case 5: newpos++; break;
                }
                if (newpos > arrcont - 1) { newpos = 0; }
                if (newpos < 0) { newpos = arrcont - 1; }
                showpos();
                if (i == 6)
                {
                    if (!File.Exists(coun[newpos]))
                    {
                        clearshow();
                        foldertocount(Prov.folderin(coun[newpos]));
                        show();
                        newpos = lastpos = 0;
                        showpos();
                    }
                    else { System.Diagnostics.Process.Start(coun[newpos]); }
                }
                if (i == 5)
                {
                    clearshow();
                    Console.Clear();
                    Prov.Create();
                    drivetocoun(Prov.alldrivers);
                    show();
                    newpos = lastpos = 0;
                    showpos();
                }


            } while (i != 8);


        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Prov.Create();
            navicon navi = new navicon();
            navi.drivetocoun(Prov.alldrivers);
            navi.main();
        }
    }
}