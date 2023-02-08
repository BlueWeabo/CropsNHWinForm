using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CropsNH
{
    public class Crop
    {
        #pragma warning disable CA2211 // Non-constant fields should not be visible
        public static BindingList<Crop> Crops = new();
#pragma warning restore CA2211 // Non-constant fields should not be visible
        private static MySqlCommand mCommand = new("",Program.mConnection);
        public int cropbreed=0;
        public int CropID { get; set; }
		public string? CropName { get; set; }
		public int Tier { get; set; }
		public string? Requirements { get; set; }
		public string? ParentOne { get; set; }
        public string? ParentTwo { get; set; }

        public float MutationChance { get; set; }

		public static void AddCrop(string cropName, int tier, string requirements, int cropBreedID)
		{
            mCommand.CommandText = "Insert into Crops(cropName, cropTier, cropRequirements, cropBreedID)" +
                "values (@cropName, @cropTier, @cropRequirements, @cropBreed);";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropName", cropName);
            mCommand.Parameters.AddWithValue("@cropTier", tier);
            mCommand.Parameters.AddWithValue("@cropRequirements", requirements);
			mCommand.Parameters.AddWithValue("@cropBreed", cropBreedID);
			mCommand.ExecuteNonQuery();
        }

        public static void AddCropWithoutBreed(string cropName, int tier, string requirements)
        {
            mCommand.CommandText = "Insert into Crops(cropName, cropTier, cropRequirements)" +
                "values (@cropName, @cropTier, @cropRequirements);";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropName", cropName);
            mCommand.Parameters.AddWithValue("@cropTier", tier);
            mCommand.Parameters.AddWithValue("@cropRequirements", requirements);
            mCommand.ExecuteNonQuery();
        }

        public static string[] GetCropNames()
		{
            List<string> crops = new()
            {
                "None"
            };
            mCommand.CommandText = "Select cropName from Crops";
            mCommand.Parameters.Clear();
            MySqlDataReader rdr = mCommand.ExecuteReader();
			using (rdr)
			{
				while(rdr.Read())
				{
					crops.Add((string)rdr["cropName"]);
				}
			}
            rdr.Close();
			return crops.ToArray();
		}

		public static int GetCropIDFromName(string cropName)
		{
            int id = 0;
            mCommand.CommandText = "Select id from Crops where cropName = @cropName;";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropName", cropName);
            MySqlDataReader rdr = mCommand.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    id = (int)rdr["id"];
                }
            }
            return id;
        }

        public static string GetCropNameFromID(int cropID)
        {
            string name = "";
            mCommand.CommandText = "Select cropName from Crops where id = @cropID;";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropID", cropID);
            MySqlDataReader rdr = mCommand.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    name = (string)rdr["cropName"];
                }
            }
            rdr.Close();
            return name;
        }

        public static string GetCropRequirementsFromID(int aCropID)
        {
            string requirements = "";
            mCommand.CommandText = "Select cropRequirements from Crops where id = @cropID;";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropID", aCropID);
            MySqlDataReader rdr = mCommand.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    requirements = rdr["cropRequirements"] is DBNull ? "" : (string)rdr["cropRequirements"];
                }
            }
            rdr.Close();
            return requirements;
        }

        public static int GetCropTierFromID(int aCropID)
        {
            int tier = 0;
            mCommand.CommandText = "Select cropTier from Crops where id = @cropID;";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropID", aCropID);
            MySqlDataReader rdr = mCommand.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    tier = (int)rdr["cropTier"];
                }
            }
            rdr.Close();
            return tier;
        }

        public static Crop GetWholeCropFromName(string aCropName)
        {
            Crop tCrop = new();
            mCommand.CommandText = "Select * from Crops where cropName = @cropName";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropName", aCropName);
            MySqlDataReader rdr = mCommand.ExecuteReader();
            while (rdr.Read())
            {
                tCrop.CropID = (int)rdr["id"];
                tCrop.CropName = (string)rdr["cropName"];
                tCrop.Tier = (int)rdr["cropTier"];
                tCrop.Requirements = rdr["cropRequirements"] is DBNull ? "" : (string)rdr["cropRequirements"];
                tCrop.cropbreed = rdr["cropBreedID"] is DBNull ? 0 : (int)rdr["cropBreedID"];
            }
            rdr.Close();
            return tCrop;
        }

        public static void UpdateCrop(Crop aCrop)
        {
            mCommand.CommandText = "Update Crops " +
                "set cropName = @cropName, " +
                "cropTier = @cropTier, " +
                "cropRequirements = @cropRequirements " +
                "where id = @id;";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropName", aCrop.CropName);
            mCommand.Parameters.AddWithValue("@cropTier", aCrop.Tier);
            mCommand.Parameters.AddWithValue("@cropRequirements", aCrop.Requirements);
            mCommand.Parameters.AddWithValue("@id", aCrop.CropID);
            mCommand.ExecuteNonQuery();
            if (aCrop.cropbreed != 0)
            {
                if (aCrop.ParentOne.Equals("None") || aCrop.ParentTwo.Equals("None"))
                {
                    mCommand.CommandText = "Update Crops set cropBreedID = null where id = @id;";
                    mCommand.Parameters.Clear();
                    mCommand.Parameters.AddWithValue("@id", aCrop.CropID);
                    mCommand.ExecuteNonQuery();
                    CropBreed.RemoveCropBreed(aCrop.cropbreed);
                }
                else
                {
                    mCommand.CommandText = "Update CropBreed " +
                        "set parentOneID = @parentOneID, " +
                        "parentTwoID = @parentTwoID " +
                        "where cropBredID = @cropBredID;";
                    mCommand.Parameters.Clear();
                    int p1 = GetCropIDFromName(aCrop.ParentOne is null ? "" : aCrop.ParentOne);
                    int p2 = GetCropIDFromName(aCrop.ParentTwo is null ? "" : aCrop.ParentTwo);
                    mCommand.Parameters.AddWithValue("@parentOneID", p1);
                    mCommand.Parameters.AddWithValue("@parentTwoID", p2);
                    mCommand.Parameters.AddWithValue("@cropBredID", aCrop.CropID);
                    mCommand.ExecuteNonQuery();
                }
            }
            else
            {
                if (aCrop.ParentOne == null || aCrop.ParentTwo == null || aCrop.ParentOne.Equals("None") || aCrop.ParentTwo.Equals("None")) return;
                CropBreed.AddCropBreed(GetCropIDFromName(aCrop.ParentOne), GetCropIDFromName(aCrop.ParentTwo));
                CropBreed.UpdateCropBredIDWithCropBreedID(aCrop.CropID,
                    CropBreed.GetCropBreedIDFromParents(GetCropIDFromName(aCrop.ParentOne), GetCropIDFromName(aCrop.ParentTwo)));
                aCrop.cropbreed = CropBreed.GetCropBreedIDFromCropBredID(aCrop.CropID);
                SetCropBreedIDForCropID(aCrop.CropID, aCrop.cropbreed);
            }
        }

        public static void SetCropBreedIDForCropID(int cropID, int cropBreedID)
        {
            mCommand.CommandText = "Update Crops set cropBreedID = @cropBreedID where id = @cropID;";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropBreedID", cropBreedID);
            mCommand.Parameters.AddWithValue("@cropID", cropID);
            mCommand.ExecuteNonQuery();
        }

        public static void DeleteCropWithName(string aCropName)
        {
            Crop tCrop = GetWholeCropFromName(aCropName);
            if (tCrop.cropbreed != 0)
            {
                mCommand.CommandText = "Update Crops set cropBreedID = @cropBreed where id = @id;";
                mCommand.Parameters.Clear();
                mCommand.Parameters.AddWithValue("@cropBreed", 1);
                mCommand.Parameters.AddWithValue("@id", tCrop.CropID);
                mCommand.ExecuteNonQuery();
                CropBreed.RemoveCropBreed(tCrop.cropbreed);

            }
            mCommand.CommandText = "Delete from Crops where cropName = @cropName;";
            mCommand.Parameters.Clear();
            mCommand.Parameters.AddWithValue("@cropName", aCropName);
            mCommand.ExecuteNonQuery();
        }

        public static void LoadCropList()
        {
            Crops.Clear();
            mCommand.CommandText = "Select * from Crops;";
            mCommand.Parameters.Clear();
            MySqlDataReader rdr = mCommand.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    Crop crop = new()
                    {
                        CropID = (int)rdr["id"],
                        CropName = (string)rdr["cropName"],
                        Tier = (int)rdr["cropTier"],
                        Requirements = rdr["cropRequirements"] is DBNull ? "" : (string)rdr["cropRequirements"],
                        cropbreed = rdr["cropBreedID"] is DBNull ? 0 : (int)rdr["cropBreedID"]
                    };
                    crop.MutationChance = (10000 - (crop.Tier - 1) * 500) / 10000.0f;
                    Crops.Add(crop);
                }
            }
            rdr.Close();
            foreach (Crop crop in Crops)
            {
                if (crop.cropbreed != 0)
                {
                    int[] parents = CropBreed.GetParentIDsFromCropBreedID(crop.cropbreed);
                    string parentOne = GetCropNameFromID(parents[0]);
                    string parentTwo = GetCropNameFromID(parents[1]);
                    crop.ParentOne = parentOne;
                    crop.ParentTwo = parentTwo;
                }
                else
                {
                    crop.ParentOne = "None";
                    crop.ParentTwo = "None";
                }
            }
        }
	}
}
