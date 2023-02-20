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
        private static MySqlCommand command = new("", Program.mConnection);
        public int CropBreedID { get; set; }
        public int CropBredID { get; set; }
        public int ParentOne { get; set; }
        public int ParentTwo { get; set; }

        // Adds a crop breed with the two parents
        public static void AddCropBreed(int parentOneID, int parentTwoID)
        {
            command.CommandText = "Insert into CropBreed(parentOneID, parentTwoID) " +
                "values (@parentOne, @parentTwo);";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@parentOne", parentOneID);
            command.Parameters.AddWithValue("@parentTwo", parentTwoID);
            command.ExecuteNonQuery();
        }

        // Gets the cropbreedID from the parents as they are a unique pair
        public static int GetCropBreedIDFromParents(int parentOneID, int parentTwoID)
        {
            command.CommandText = "Select id from CropBreed where parentOneID = @parentOne and parentTwoID = @parentTwo;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@parentOne", parentOneID);
            command.Parameters.AddWithValue("@parentTwo", parentTwoID);
            MySqlDataReader rdr = command.ExecuteReader();
            int id = 0;
            using (rdr)
            {
                while (rdr.Read())
                {
                    id = (int) rdr["id"];
                }
            }
            return id;
        }

        // Adds the Crop, which was made from the two parents in the cropBreed
        public static void UpdateCropBredIDWithCropBreedID(int cropBredID, int cropBreedID)
        {
            command.CommandText = "Update CropBreed set cropBredID = @cropBredID where id = @id;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropBredID", cropBredID);
            command.Parameters.AddWithValue("@id", cropBreedID);
            command.ExecuteNonQuery();
        }

        // Returns an array of the two parent's ids
        public static int[] GetParentIDsFromCropBreedID(int cropBreedID)
        {
            int[] parentIDs = new int[2];
            command.CommandText = "Select parentOneID, parentTwoID from CropBreed where id = @cropBreedID";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropBreedID", cropBreedID);
            MySqlDataReader rdr = command.ExecuteReader();
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

        // Returns the cropbreed id from the crop which it makes
        public static int GetCropBreedIDFromCropBredID(int cropBredID)
        {
            int cropBreedID = 0;
            command.CommandText = "Select id from CropBreed where cropBredID = @cropBredID";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropBredID", cropBredID);
            MySqlDataReader rdr = command.ExecuteReader();
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

        // Removes a crop Breed from the table
        public static void RemoveCropBreed(int aCropBreedID)
        {
            command.CommandText = "Delete from CropBreed where id = @cropBreedID;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropBreedID", aCropBreedID);
            command.ExecuteNonQuery();
        }
    }
}
