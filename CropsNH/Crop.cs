using System.ComponentModel;
using MySql.Data.MySqlClient;

namespace CropsNH
{
    public class Crop
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static BindingList<Crop> Crops = new();
#pragma warning restore CA2211 // Non-constant fields should not be visible
        private static readonly MySqlCommand command = new("", Program.mConnection);
        public int cropbreed = 0;
        public int CropID { get; set; }
        public string? CropName { get; set; }
        public int Tier { get; set; }
        public string? Requirements { get; set; }
        public string? ParentOne { get; set; }
        public string? ParentTwo { get; set; }

        public float MutationChance { get; set; }

        // Adds a crop with the given parameters. This adds a crop which comes from 2 parents
        public static void AddCrop(string cropName, int tier, string requirements, int cropBreedID)
        {
            command.CommandText = "Insert into Crops(cropName, cropTier, cropRequirements, cropBreedID)" +
                "values (@cropName, @cropTier, @cropRequirements, @cropBreed);";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropName", cropName);
            command.Parameters.AddWithValue("@cropTier", tier);
            command.Parameters.AddWithValue("@cropRequirements", requirements);
            command.Parameters.AddWithValue("@cropBreed", cropBreedID);
            command.ExecuteNonQuery();
        }

        // Adds a crop with the given parameters. This adds a crop, which has no parents
        public static void AddCropWithoutBreed(string cropName, int tier, string requirements)
        {
            command.CommandText = "Insert into Crops(cropName, cropTier, cropRequirements)" +
                "values (@cropName, @cropTier, @cropRequirements);";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropName", cropName);
            command.Parameters.AddWithValue("@cropTier", tier);
            command.Parameters.AddWithValue("@cropRequirements", requirements);
            command.ExecuteNonQuery();
        }

        // Returns all crop names in an array form
        public static string[] GetCropNames()
        {
            List<string> crops = new()
            {
                "None"
            };
            command.CommandText = "Select cropName from Crops";
            command.Parameters.Clear();
            MySqlDataReader rdr = command.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    crops.Add((string)rdr["cropName"]);
                }
            }
            rdr.Close();
            return crops.ToArray();
        }

        // returns the crop's id from its name
        public static int GetCropIDFromName(string cropName)
        {
            int id = 0;
            command.CommandText = "Select id from Crops where cropName = @cropName;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropName", cropName);
            MySqlDataReader rdr = command.ExecuteReader();
            using (rdr)
            {
                while (rdr.Read())
                {
                    id = (int)rdr["id"];
                }
            }
            rdr.Close();
            command.Parameters.Clear();
            return id;
        }

        // returns the crop's name from its id
        public static string GetCropNameFromID(int cropID)
        {
            string name = "";
            command.CommandText = "Select cropName from Crops where id = @cropID;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropID", cropID);
            MySqlDataReader rdr = command.ExecuteReader();
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

        // Returns the crop requirements from the crop id
        public static string GetCropRequirementsFromID(int aCropID)
        {
            string requirements = "";
            command.CommandText = "Select cropRequirements from Crops where id = @cropID;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropID", aCropID);
            MySqlDataReader rdr = command.ExecuteReader();
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

        // Returns the crop's tier from its id
        public static int GetCropTierFromID(int aCropID)
        {
            int tier = 0;
            command.CommandText = "Select cropTier from Crops where id = @cropID;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropID", aCropID);
            MySqlDataReader rdr = command.ExecuteReader();
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

        // Returns the crop which was registered with the given name
        public static Crop GetWholeCropFromName(string aCropName)
        {
            Crop tCrop = new();
            command.CommandText = "Select * from Crops where cropName = @cropName";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropName", aCropName);
            MySqlDataReader rdr = command.ExecuteReader();
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

        // Updates the whole crop at its id. This changes everything from name to crop breed
        public static void UpdateCrop(Crop aCrop)
        {
            command.CommandText = "Update Crops " +
                "set cropName = @cropName, " +
                "cropTier = @cropTier, " +
                "cropRequirements = @cropRequirements " +
                "where id = @id;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropName", aCrop.CropName);
            command.Parameters.AddWithValue("@cropTier", aCrop.Tier);
            command.Parameters.AddWithValue("@cropRequirements", aCrop.Requirements);
            command.Parameters.AddWithValue("@id", aCrop.CropID);
            command.ExecuteNonQuery();
            if (aCrop.cropbreed != 0)
            {
                // If One of the parents was set to none or removed. We need to remove the breeding id in the CropBreed table
                // Otherwise we edit the crop breed with the new parents' ids
                if (aCrop.ParentOne == null || aCrop.ParentTwo == null || aCrop.ParentOne.Equals("None") || aCrop.ParentTwo.Equals("None"))
                {
                    command.CommandText = "Update Crops set cropBreedID = null where id = @id;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", aCrop.CropID);
                    command.ExecuteNonQuery();
                    CropBreed.RemoveCropBreed(aCrop.cropbreed);
                }
                else
                {
                    // If the crop Breed was changed. wewill update it here
                    int p1 = GetCropIDFromName(aCrop.ParentOne is null ? "" : aCrop.ParentOne);
                    int p2 = GetCropIDFromName(aCrop.ParentTwo is null ? "" : aCrop.ParentTwo);
                    command.Parameters.Clear();
                    command.CommandText = "Update CropBreed " +
                        "set parentOneID = @parentOneID, " +
                        "parentTwoID = @parentTwoID " +
                        "where cropBredID = @cropBredID;";
                    command.Parameters.AddWithValue("@parentOneID", p1);
                    command.Parameters.AddWithValue("@parentTwoID", p2);
                    command.Parameters.AddWithValue("@cropBredID", aCrop.CropID);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                // If there was no crop breed, we need to check if one was added and add it as a new crop breed
                if (aCrop.ParentOne == null || aCrop.ParentTwo == null || aCrop.ParentOne.Equals("None") || aCrop.ParentTwo.Equals("None")) return;
                CropBreed.AddCropBreed(GetCropIDFromName(aCrop.ParentOne), GetCropIDFromName(aCrop.ParentTwo));
                CropBreed.UpdateCropBredIDWithCropBreedID(aCrop.CropID,
                    CropBreed.GetCropBreedIDFromParents(GetCropIDFromName(aCrop.ParentOne), GetCropIDFromName(aCrop.ParentTwo)));
                aCrop.cropbreed = CropBreed.GetCropBreedIDFromCropBredID(aCrop.CropID);
                SetCropBreedIDForCropID(aCrop.CropID, aCrop.cropbreed);
            }
        }

        // Sets a new cropbreedID from the crop's id
        public static void SetCropBreedIDForCropID(int cropID, int cropBreedID)
        {
            command.CommandText = "Update Crops set cropBreedID = @cropBreedID where id = @cropID;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropBreedID", cropBreedID);
            command.Parameters.AddWithValue("@cropID", cropID);
            command.ExecuteNonQuery();
        }

        // Deletes the cropwith the given name
        public static void DeleteCropWithName(string aCropName)
        {
            Crop tCrop = GetWholeCropFromName(aCropName);
            // If there is a crop breed we must first remove it by changing the crop's cropBreedID reference key to 1, to allows us to delete the crop
            if (tCrop.cropbreed != 0)
            {
                command.CommandText = "Update Crops set cropBreedID = @cropBreed where id = @id;";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@cropBreed", 1);
                command.Parameters.AddWithValue("@id", tCrop.CropID);
                command.ExecuteNonQuery();
                CropBreed.RemoveCropBreed(tCrop.cropbreed);

            }
            command.CommandText = "Delete from Crops where cropName = @cropName;";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@cropName", aCropName);
            command.ExecuteNonQuery();
        }

        // Loads all crops into a list, which is then used to put int a data grid view
        public static void LoadCropList()
        {
            Crops.Clear();
            command.CommandText = "Select * from Crops;";
            command.Parameters.Clear();
            MySqlDataReader rdr = command.ExecuteReader();
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
            // After we have gotten each crop and their cropBreed, we will go over the list we have and then get the parents of said crops to give
            // that information to the data grid view. If the cropbreed equals 0 then there is none, and thus there are no parents
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
