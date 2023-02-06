using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CropsNH
{
    public class CropBreed
    {
        private static MySqlCommand mCommand = new("", Program.mConnection);
        public int CropBreedID { get; set; }
        public int CropBredID { get; set; }
        public int ParentOne { get; set; }
        public int ParentTwo { get; set; }

        public static void AddCropBreed(int parentOneID, int parentTwoID)
        {
            MySqlCommand cmd = new()
            {
                Connection = Program.mConnection,
                CommandText = "Insert into CropBreed(parentOneID, parentTwoID) " +
                "values (@parentOne, @parentTwo);"
            };
            cmd.Parameters.AddWithValue("@parentOne", parentOneID);
            cmd.Parameters.AddWithValue("@parentTwo", parentTwoID);
            cmd.ExecuteNonQuery();
        }
        public static int GetCropBreedIDFromParents(int parentOneID, int parentTwoID)
        {
            int id = 0;
            MySqlCommand cmd = new()
            {
                Connection = Program.mConnection,
                CommandText = "Select id from CropBreed where parentOneID = @parentOne and parentTwoID = @parentTwo;"
            };
            cmd.Parameters.AddWithValue("@parentOne", parentOneID);
            cmd.Parameters.AddWithValue("@parentTwo", parentTwoID);
            MySqlDataReader rdr = cmd.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    id = (int) rdr["id"];
                }
            }
            return id;
        }

        public static void UpdateCropBredIDWithCropBreedID(int cropBredID, int cropBreedID)
        {
            MySqlCommand cmd = new()
            {
                Connection = Program.mConnection,
                CommandText = "Update CropBreed set cropBredID = @cropBredID where id = @id;"
            };
            cmd.Parameters.AddWithValue("@cropBredID", cropBredID);
            cmd.Parameters.AddWithValue("@id", cropBreedID);
            cmd.ExecuteNonQuery();
        }

        public static int[] GetParentIDsFromCropBreedID(int cropBreedID)
        {
            int[] parentIDs = new int[2];
            MySqlCommand cmd = new()
            {
                CommandText = "Select parentOneID, parentTwoID from CropBreed where id = @cropBreedID",
                Connection = Program.mConnection
            };
            cmd.Parameters.AddWithValue("@cropBreedID", cropBreedID);
            MySqlDataReader rdr = cmd.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    parentIDs[0] = (int) rdr["parentOneID"];
                    parentIDs[1] = (int) rdr["parentTwoID"];
                }
            }
            rdr.Close();
            return parentIDs;
        }
        public static int GetCropBreedIDFromCropBredID(int cropBredID)
        {
            int cropBreedID = 0;
            MySqlCommand cmd = new()
            {
                CommandText = "Select id from CropBreed where cropBredID = @cropBredID",
                Connection = Program.mConnection
            };
            cmd.Parameters.AddWithValue("@cropBredID", cropBredID);
            MySqlDataReader rdr = cmd.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    cropBreedID = (int)rdr["id"];
                }
            }
            rdr.Close();
            return cropBreedID;
        }

        public static void RemoveCropBreed(int aCropBreedID)
        {
            mCommand.CommandText = "Delete from CropBreed where id = @cropBreedID;";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropBreedID", aCropBreedID);
            mCommand.ExecuteNonQuery();
        }
    }
}
