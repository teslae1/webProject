using bbsongPro.Aquantainces;
using bbsongPro.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace bbsongPro.Foundation
{
    public class OrderFoundation
    {

        /// <summary>
        /// Inserts order into database by accessing two tabels: 
        /// Order and SongParts
        /// </summary>
        /// <param name="orderToCreate"></param>
        /// <returns></returns>
        public void Create(Order orderToCreate)
        {

            int assignedId = 0;
            using (var conn = new MySqlConnection(Startup.ConnString))
            {
                conn.Open();
                
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;


                    //order insert query
                    cmd.Parameters.AddWithValue("@email", orderToCreate.Email);

                    cmd.Parameters.AddWithValue("@comment", orderToCreate.Comment);

                    cmd.Parameters.AddWithValue("@payment_Received", 0);

                    cmd.CommandText = $"INSERT INTO `Order` (`email`, `comment`, `payment_Received`) " +
                        $"VALUES (@email, @comment, @payment_Received);";



                    //songParts insert query
                    for (int i = 0; i < orderToCreate.SongParts.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@link" + (i + 1), orderToCreate.SongParts[i].Link);

                        cmd.Parameters.AddWithValue("@from" + (i + 1), orderToCreate.SongParts[i].From);

                        cmd.Parameters.AddWithValue("@To" + (i + 1), orderToCreate.SongParts[i].To);

                    }

                    string songPartsInsertQuery = " INSERT INTO `SongParts` (`order_Id`, `link`, `from`, `to`) VALUES ";

                    for (int i = 1; i <= orderToCreate.SongParts.Count; i++)
                    {
                        songPartsInsertQuery += $"(LAST_INSERT_ID(), @link{i}, @from{i}, @to{i})";
                        if (i == orderToCreate.SongParts.Count)
                            songPartsInsertQuery += "; ";
                        else
                            songPartsInsertQuery += ", ";
                    }

                    cmd.CommandText += songPartsInsertQuery;


                    //get order_id query
                    string getLastIdQuery = " SELECT LAST_INSERT_ID()";
                    cmd.CommandText += getLastIdQuery;

                    assignedId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }


            orderToCreate.Id = assignedId;
        }
        public List<Order> GetAll()
        {
            var orders = new List<Order>();

            using (var conn = new MySqlConnection(Startup.ConnString))
            {
                conn.Open();

                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM `Order`" +
                        " INNER JOIN SongParts " +
                        " on SongParts.order_Id = Order.order_Id ;";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("order_Id");
                                if (orders.Any(order => order.Id == id))
                                {
                                    //add songPart to exisiting order
                                    var order = orders.Single(order => order.Id == id);
                                    var songPart = new SongPart(reader.GetString("link"), reader.GetString("from"), reader.GetString("to"));
                                    order.SongParts.Add(songPart);
                                }
                                else
                                {
                                    //create new order and add songpart
                                    var order = new Order(
                                        reader.GetInt32("order_Id"),
                                        reader.GetString("email"),
                                        reader.IsDBNull(reader.GetOrdinal("comment")) ? null : reader.GetString("comment"),
                                        reader.GetDateTime("creationDate"),
                                        reader.GetBoolean("order_Completed"),
                                        reader.GetBoolean("payment_Received")
                                        );
                                    order.SongParts = new List<SongPart>() { new SongPart(reader.GetString("link"), reader.GetString("from"), reader.GetString("to")) };
                                    orders.Add(order);
                                }
                            }
                        }
                    }


                    return orders;
                }
            }
        }

        public void MarkAsComplete(Order order)
        {
            using(var conn = new MySqlConnection(Startup.ConnString))
            {
                    conn.Open();

                using (var cmd = new MySqlCommand("UPDATE `Order` SET `order_Completed`=1 WHERE order_Id = @order_Id ", conn))
                {
                    cmd.Parameters.AddWithValue("@order_Id", order.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void MarkAsPaymentReceived(Order order)
        {
            using (var conn = new MySqlConnection(Startup.ConnString))
            {
                conn.Open();
                using(var cmd = new MySqlCommand("UPDATE `Order` SET `payment_Received`=1 WHERE order_Id = @order_Id ", conn))
                {
                    cmd.Parameters.AddWithValue("@order_Id", order.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
