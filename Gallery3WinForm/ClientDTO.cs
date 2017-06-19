using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredGamesWindowsform
{
    //////////////PublisherDTO////////////////////////////////////
    public class clsPublisher
    {
            public string Name { get; set; }
            public string Email { get; set; }
            public List<clsAllGames> GamesList { get; set; }
    }
    /////////////////ALLGAMESDTO/////////////////////////////////
    public class clsAllGames
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public int? Players { get; set; }
        public string Type { get; set; }
        public string Genre { get; set; }
        public string PublisherName { get; set; }

        public static readonly string FACTORY_PROMPT = "Enter P for Party Game and B for Board game";

        public static clsAllGames NewWork(char prChoice)
        {
            return new clsAllGames() { Type = prChoice.ToString() };
        }
        public override string ToString()
        {
            return Name + "\t" + Date.ToShortDateString();
        }
    }
    /////////////////ORDER///////////////////////////////////////
    public class clsOrder
    {
        public string OrderID { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string GameName { get; set; }

    }
}
