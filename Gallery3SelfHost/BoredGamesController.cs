using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


namespace BoredGamesSelfHost
{
    public class BoredGamesController : ApiController
    {

        /////////////////////////////////////////SELECTING////////////////////////////////////////////////////////
        public List<string> GetPublisherName()
        {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT Name FROM Publishers", null);
            List<string> lcNames = new List<string>();
            foreach (DataRow dr in lcResult.Rows)
                lcNames.Add((string)dr[0]);
            return lcNames;
        }
        public List<string> GetOrderInfo()
        {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT OrderID FROM Orders", null);
            List<string> lcOrders = new List<string>();
            foreach (DataRow dr in lcResult.Rows)
                lcOrders.Add((string)dr[0]);
            return lcOrders;
        }
        public clsOrders GetOrder(string OrderID)
        {
            DataTable lcResult =
            clsDbConnection.GetDataTable("SELECT * FROM Orders WHERE OrderID = @OrderID", PrepareOrderIDParameters(OrderID));
            if (lcResult.Rows.Count > 0)
                return new clsOrders()
                {

                    OrderID = (string)lcResult.Rows[0]["OrderID"],
                    Email = (string)lcResult.Rows[0]["Email"],
                    Date = Convert.ToDateTime(lcResult.Rows[0]["Date"]),
                    Address = (string)lcResult.Rows[0]["Address"],
                    Quantity = Convert.ToInt32(lcResult.Rows[0]["Quantity"]),
                    Price =  Convert.ToDecimal(lcResult.Rows[0]["Price"]),
                    GameName = (string)lcResult.Rows[0]["GameName"],

                };
            else return null;
        }
        public clsPublisher GetPublishers(string Name)
        {
            DataTable lcResult =
            clsDbConnection.GetDataTable("SELECT * FROM Publishers WHERE Name = @Name", PreparePublisherNameParameters(Name));
            if (lcResult.Rows.Count > 0)
                return new clsPublisher()
                {

                    Name = (string)lcResult.Rows[0]["Name"],
                    Email = (string)lcResult.Rows[0]["Email"],
                    GamesList = GetPublisherProducts(Name)

                };
            else return null;
        }
        private List<clsAllGames> GetPublisherProducts(string Name)
        {
          
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Games WHERE PublisherName = @Name", PreparePublisherNameParameters(Name));
            List<clsAllGames> lcWorks = new List<clsAllGames>();
            foreach (DataRow dr in lcResult.Rows)
                lcWorks.Add(dataRowToAllGames(dr));
            return lcWorks;

        }
        //////////////////////////////////////////PREPARE DATA////////////////////////////////////////////////////
        private Dictionary<string, object> PreparePublisherNameParameters(string name)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1)
            {
                { "Name", name }
            };
            return par;
        }
        private Dictionary<string, object> PrepareOrderIDParameters(string OrderID)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1)
            {
                { "OrderID", OrderID }
            };
            return par;
        }
        private Dictionary<string, object> PrepareGameDeletionParameters(string Name, string PublisherName)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(2)
            {
                { "Name", Name },
                { "ArtistName", PublisherName }
            };
            return par;
        }
        private Dictionary<string, object> PreparePublisherParameters(clsPublisher prPublisher)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(2)
            {
                { "Name", prPublisher.Name },
                { "Email", prPublisher.Email }
            };
            return par;

        }
        private Dictionary<string, object> PrepareGameParameters(clsAllGames prGame)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(8)
            {
                { "Name", prGame.Name },
                { "Date", prGame.Date },
                { "Price", prGame.Price },
                { "Quantity", prGame.Quantity },
                { "Players", prGame.Players },
                { "Type", prGame.Type },
                { "Genre", prGame.Genre },
                { "PublisherName", prGame.PublisherName }
            };
            return par;
        }
        private Dictionary<string, object> PrepareOrderParameters(clsOrders prOrder)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(7)
            {
                { "OrderID", prOrder.OrderID },
                { "Email", prOrder.Email },
                { "Date", prOrder.Date },
                { "address", prOrder.Address },
                { "Quantity", prOrder.Quantity },
                { "Price", prOrder.Price },
                { "GameName", prOrder.GameName }

            };
            return par;
        }
        private clsAllGames dataRowToAllGames(DataRow prDataRow)
        {
            return new clsAllGames()
            {

                Name = Convert.ToString(prDataRow["Name"]),
                Date = Convert.ToDateTime(prDataRow["Date"]),
                Price = Convert.ToDecimal(prDataRow["Price"]),
                Quantity = Convert.ToInt32(prDataRow["Quantity"]),
                Players = prDataRow["Players"] is DBNull ? (int?)null : Convert.ToInt32(prDataRow["Players"]),
                Type = Convert.ToString(prDataRow["Type"]),
                Genre = Convert.ToString(prDataRow["Genre"]),
                PublisherName = Convert.ToString(prDataRow["PublisherName"])

            };

        }
        ///////////////////////////////////////////INSERT//////////////////////////////////////////////////////////
        public string PostGames(clsAllGames prGame)
        {   
            try
            {
                int lcRecCount = clsDbConnection.Execute("INSERT INTO Games " +
                "(Name, Date, Type, Price, Quantity, Players, Genre, PublisherName) " +
                "VALUES (@Name, @Date, @Type, @Price, @Quantity, @Players, @Genre, @PublisherName)",
                PrepareGameParameters(prGame));
                if (lcRecCount == 1)
                    return "One Game inserted";
                else
                    return "Unexpected Game insert count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }
        public string PostOrder(clsOrders prOrder)
        {
            try
            {
                int lcRecCount = clsDbConnection.Execute("INSERT INTO Orders " +
                "(OrderID, Email, Date, Address, Quantity, Price, GameName) " +
                "VALUES (@OrderID, @Email, @Date, @Address, @Quantity, @Price, @GameName)",
                PrepareOrderParameters(prOrder));
                if (lcRecCount == 1)
                    return "One Order inserted";
                else
                    return "Unexpected Game insert count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }
        public string PostPublisher(clsPublisher prPublishers)
        {
            try
            {
                if (clsDbConnection.Execute
                    ("INSERT INTO Publishers (Name, Email) VALUES (@Name, @Email);",
                    PreparePublisherParameters(prPublishers)) == 1)
                {
                    return "one publisher Added";
                }

                else return "something went very very wrong....";

            }
            catch (Exception ex)
            {

                return ex.GetBaseException().Message;

            }
        }
        /////////////////////////////////////////UPDATE////////////////////////////////////////////////////////////
        public string PutPublisher(clsPublisher prPublisher)
        {
            try
            {

                int lcRecCount = clsDbConnection.Execute(
                    "UPDATE Publishers SET Email = @Email WHERE Name = @Name",
                    PreparePublisherParameters(prPublisher));
                if (lcRecCount == 1)
                    return "One publisher updated";
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }

        }
        public string PutOrder(clsAllGames prGame)
        {
            try
            {

                int lcRecCount = clsDbConnection.Execute(
                    "UPDATE Orders SET  Date = @Date, Type = @Type, Price = @Price, Quantity = @Quantity, Players = @Players, Genre = @Genre WHERE Name = @Name AND PublisherName = @PublisherName ",
                    PrepareGameParameters(prGame));
                if (lcRecCount == 1)
                    return "One Game updated";
                else
                    return "Unexpected Work update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }

        }
        ////////////////////////////////////////DELETE/////////////////////////////////////////////////////////////
        public string DeleteGame(string Name, string PublisherName)
        {
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    ("DELETE FROM Games WHERE Name = @Name AND PublisherName = @PublisherName"),
                    PrepareGameDeletionParameters(Name, PublisherName));

                if (lcRecCount == 1)

                    return "One Game DELETED";
                else

                    return "Unexpected artist update count: " + lcRecCount;

            }
            catch (Exception ex)
            {

                return ex.GetBaseException().Message;
            }
        }
        public string DeletePublisher(string Name)
        {
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    ("DELETE FROM Publishers WHERE Name = @name"),
                    PreparePublisherNameParameters(Name));
                    if (lcRecCount == 1)

                    return "One publisher Deleted";
                else

                    return "unexpected Artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {

                return ex.GetBaseException().Message;
            }
        }
        public string DeleteOrder(string OrderID)
        {
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    ("DELETE FROM Orders WHERE OrderID = @OrderID"),
                    PrepareOrderIDParameters(OrderID));
                if (lcRecCount == 1)

                    return "One Order Deleted";
                else

                    return "unexpected Artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {

                return ex.GetBaseException().Message;
            }
        }

    }
}


