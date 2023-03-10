namespace CropsNH
{
    public partial class MainMenu : Form
    {
        public bool mChanged = true;
        private bool mAdministrator = false;
        public MainMenu()
        {
            InitializeComponent();
            ContainerAll.Hide();
        }

        // Open the add crop menu
        private void AddCropButton_Click(object sender, EventArgs e)
        {
            LoadCrops();
            ContainerAll.Show();
            ContainerAll.Panel2.Show();
            ContainerAll.Panel1.Hide();
            ContainerAddDisplay.Panel1.Show();
            ContainerAddDisplay.Panel2.Hide();
            ContainerAll.SplitterDistance = 0;
            ContainerAddDisplay.SplitterDistance = ContainerAll.Width;
        }

        // Reloads the data grid if any values have changed
        private void ReloadDataGrid()
        {
            if (mChanged)
            {
                Crop.LoadCropList();
                mChanged = false;
            }
            CropDataGrid.DataSource = new BindingSource(Crop.Crops, null);
        }

        // Opens the Data grid view to as its own element
        private void DisplayCropsButton_Click(object sender, EventArgs e)
        {
            ReloadDataGrid();
            ContainerAll.Show();
            ContainerAll.Panel2.Show();
            ContainerAll.Panel1.Hide();
            ContainerAddDisplay.Panel2.Show();
            ContainerAddDisplay.Panel1.Hide();
            ContainerAll.SplitterDistance = 0;
            ContainerAddDisplay.SplitterDistance = 0;

        }

        // Whenever the main menu loads we must open the login menu and check if the user has logged in with an admin account
        // If they haven't we disabled the edit, delete and add buttons
        private void MainMenu_Load(object sender, EventArgs e)
        {
            Login login = new();
            login.ShowDialog();
            if (login.id > 0) mAdministrator = true;
            if (!mAdministrator)
            {
                CropAddButton.Enabled = false;
                CropEditButton.Enabled = false;
                CropDeleteButton.Enabled = false;
            }
            MainMenu_Resize(sender, e);
        }

        // Saves a new crop with the values put into the parameter boxes
        private void SaveCrop_Click(object sender, EventArgs e)
        {
            string parentOne = (string)Save_CropParentOne.SelectedItem;
            string parentTwo = (string)Save_CropParentTwo.SelectedItem;
            string cropName = Save_CropNameTextBox.Text;
            int cropTier = (int)Save_CropTierNumber.Value;
            string cropRequirements = Save_CropRequirementsTextBox.Text;
            object? cropBreedID = null;
            // We must check if the current crop's name is already registered or aany of the two parent are not found inside of it;
            if (Save_CropParentOne.Items.Contains(cropName) || !Save_CropParentOne.Items.Contains(parentOne) || !Save_CropParentOne.Items.Contains(parentTwo))
            {
                return;
            }

            // If the parents are selected and they aren't equal to None we must not save it as a crop breed
            if (parentOne != null && parentTwo != null && !parentOne.Equals("None") && !parentOne.Equals("None"))
            {
                int parentOneID = Crop.GetCropIDFromName(parentOne);
                int parentTwoID = Crop.GetCropIDFromName(parentTwo);
                CropBreed.AddCropBreed(parentOneID, parentTwoID);
                cropBreedID = CropBreed.GetCropBreedIDFromParents(parentOneID, parentTwoID);
            }

            // If we got a cropbreed then we must save the crop with a crop breed
            if (cropBreedID != null)
            {
                Crop.AddCrop(cropName, cropTier, cropRequirements, (int)cropBreedID);
            }

            // Otherwise we must save it as if it doesn't have one
            else
            {
                Crop.AddCropWithoutBreed(cropName, cropTier, cropRequirements);
            }

            // if the Crop breed isn't null we can now update the corp breed it its new id
            if (cropBreedID != null)
            {
                int cropID = Crop.GetCropIDFromName(cropName);
                CropBreed.UpdateCropBredIDWithCropBreedID(cropID, (int)cropBreedID);
            }

            // Afterwards we must reload the crop items
            ReLoadCrops();
            mChanged = true;
        }

        // Loads the crop's names which currently exist
        private void LoadCrops()
        {
            Save_CropParentOne.Items.AddRange(Crop.GetCropNames());
            Save_CropParentTwo.Items.AddRange(Crop.GetCropNames());
        }

        // We clear the Items of the combo box and then load it back up
        private void ReLoadCrops()
        {
            Save_CropParentOne.Items.Clear();
            Save_CropParentTwo.Items.Clear();
            LoadCrops();
        }

        // Whenever the menu size changes we must also change the size of the text, so this long method was made
        private void MainMenu_Resize(object sender, EventArgs e)
        {
            Save_CropParentOneLabel.Font = new Font(Save_CropParentOneLabel.Font.FontFamily, Height / 95f + Width / 100f);
            Save_CropParentTwoLabel.Font = new Font(Save_CropParentTwoLabel.Font.FontFamily, Height / 95f + Width / 100f);
            Save_CropNameLabel.Font = new Font(Save_CropNameLabel.Font.FontFamily, Height / 60f + Width / 75f);
            Save_CropRequirementsLabel.Font = new Font(Save_CropRequirementsLabel.Font.FontFamily, Height / 75f + Width / 100f);
            Save_CropTierLabel.Font = new Font(Save_CropTierLabel.Font.FontFamily, Height / 75f + Width / 100f);
            Save_SaveNewCropButton.Font = new Font(Save_SaveNewCropButton.Font.FontFamily, Height / 80f + Width / 100f);
            CropAddButton.Font = new Font(CropAddButton.Font.FontFamily, Height / 80f + Width / 95f);
            CropDisplayButton.Font = new Font(CropDisplayButton.Font.FontFamily, Height / 80f + Width / 95f);
            Save_CropParentOne.Font = new Font(Save_CropParentOne.Font.FontFamily, Height / 95f + Width / 100f);
            Save_CropParentTwo.Font = new Font(Save_CropParentTwo.Font.FontFamily, Height / 95f + Width / 100f);
            Save_CropNameTextBox.Font = new Font(Save_CropNameTextBox.Font.FontFamily, Height / 60f + Width / 75f);
            Save_CropRequirementsTextBox.Font = new Font(Save_CropRequirementsTextBox.Font.FontFamily, Height / 75f + Width / 100f);
            Save_CropTierNumber.Font = new Font(Save_CropTierNumber.Font.FontFamily, Height / 75f + Width / 100f);
            CropDeleteButton.Font = new Font(CropDeleteButton.Font.FontFamily, Height / 80f + Width / 95f);
            Delete_CropLabel.Font = new Font(Delete_CropLabel.Font.FontFamily, Height / 60f + Width / 75f);
            Delete_CropToDeleteComboBox.Font = new Font(Delete_CropToDeleteComboBox.Font.FontFamily, Height / 60f + Width / 75f);
            Delete_CropDeleteButton.Font = new Font(Delete_CropDeleteButton.Font.FontFamily, Height / 60f + Width / 75f);
            CropEditButton.Font = new Font(CropEditButton.Font.FontFamily, Height / 80f + Width / 95f);
            Edit_CropParentOne.Font = new Font(Edit_CropParentOne.Font.FontFamily, (Height / 95f + Width / 100f) / 1.3f);
            Edit_CropParentTwo.Font = new Font(Edit_CropParentTwo.Font.FontFamily, (Height / 95f + Width / 100f) / 1.3f);
            Edit_CropNewName.Font = new Font(Edit_CropNewName.Font.FontFamily, (Height / 60f + Width / 75f) / 1.3f);
            Edit_CropRequirementsTextBox.Font = new Font(Edit_CropRequirementsTextBox.Font.FontFamily, (Height / 75f + Width / 100f) / 1.3f);
            Edit_CropTierNumber.Font = new Font(Edit_CropTierNumber.Font.FontFamily, (Height / 75f + Width / 100f) / 1.3f);
            Edit_CropParentOneLabel.Font = new Font(Edit_CropParentOneLabel.Font.FontFamily, (Height / 95f + Width / 100f) / 1.3f);
            Edit_CropParentTwoLabel.Font = new Font(Edit_CropParentTwoLabel.Font.FontFamily, (Height / 95f + Width / 100f) / 1.3f);
            Edit_CropNameLabel.Font = new Font(Edit_CropNameLabel.Font.FontFamily, (Height / 60f + Width / 75f) / 1.3f);
            Edit_CropRequirementsLabel.Font = new Font(Edit_CropRequirementsLabel.Font.FontFamily, (Height / 75f + Width / 100f) / 1.3f);
            Edit_CropTierLabel.Font = new Font(Edit_CropTierLabel.Font.FontFamily, (Height / 75f + Width / 100f) / 1.3f);
            Edit_CropToEditComboBox.Font = new Font(Edit_CropToEditComboBox.Font.FontFamily, (Height / 75f + Width / 100f) / 1.3f);
            Edit_CropLabel.Font = new Font(Edit_CropLabel.Font.FontFamily, (Height / 60f + Width / 75f) / 1.3f);
            Edit_SaveEditedCrop.Font = new Font(Edit_SaveEditedCrop.Font.FontFamily, (Height / 60f + Width / 75f) / 1.3f);
        }

        // Whenever a new item was selected in the crop edit combo box we must populate the other elements with the current data of said crop
        private void CropToEditComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            Edit_CropNewName.Text = Edit_CropToEditComboBox.Text;
            int cropID = Crop.GetCropIDFromName(Edit_CropToEditComboBox.Text);
            int[] parentIDs = CropBreed.GetParentIDsFromCropBreedID(CropBreed.GetCropBreedIDFromCropBredID(cropID));
            Edit_CropParentOne.Text = Crop.GetCropNameFromID(parentIDs[0]);
            Edit_CropParentTwo.Text = Crop.GetCropNameFromID(parentIDs[1]);
            Edit_CropRequirementsTextBox.Text = Crop.GetCropRequirementsFromID(cropID);
            Edit_CropTierNumber.Value = Crop.GetCropTierFromID(cropID);
        }

        // Opensthe Edit menu and the daga grid view
        private void EditButton_Click(object sender, EventArgs e)
        {
            ReloadDataGrid();
            ReloadEdit();
            ContainerAll.Show();
            ContainerAll.Panel2.Show();
            ContainerAll.Panel1.Show();
            ContainerAddDisplay.Panel2.Show();
            ContainerAddDisplay.Panel1.Hide();
            ContainerAll.SplitterDistance = ContainerAll.Width / 2;
            ContainerAddDisplay.SplitterDistance = 0;
            ContainerEditDelete.Panel1.Show();
            ContainerEditDelete.Panel2.Hide();
            ContainerEditDelete.SplitterDistance = ContainerAll.Width / 2;
        }

        // After a crop was edited we put everything to be empty
        private void ReloadEdit()
        {
            Edit_CropToEditComboBox.Items.Clear();
            Edit_CropToEditComboBox.Items.AddRange(Crop.GetCropNames());
            Edit_CropParentOne.Items.Clear();
            Edit_CropParentTwo.Items.Clear();
            Edit_CropParentOne.Items.AddRange(Crop.GetCropNames());
            Edit_CropParentTwo.Items.AddRange(Crop.GetCropNames());
            Edit_CropToEditComboBox.Text = "None";
            Edit_CropParentOne.Text = "None";
            Edit_CropParentTwo.Text = "None";
        }

        // Saves a crop with its edited values
        private void SaveEditedCrop_Click(object sender, EventArgs e)
        {
            Crop tCrop = new()
            {
                CropID = Crop.GetCropIDFromName(Edit_CropToEditComboBox.Text),
                CropName = Edit_CropNewName.Text,
                Tier = (int)Edit_CropTierNumber.Value,
                Requirements = Edit_CropRequirementsTextBox.Text,
                ParentOne = Edit_CropParentOne.Text,
                ParentTwo = Edit_CropParentTwo.Text
            };
            tCrop.cropbreed = CropBreed.GetCropBreedIDFromCropBredID(tCrop.CropID);
            Crop.UpdateCrop(tCrop);
            ReloadEdit();
            mChanged = true;
            ReloadDataGrid();
        }

        // Opens the delete menu and data grid view
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            ReloadDataGrid();
            ContainerAll.Show();
            ContainerAll.Panel2.Show();
            ContainerAll.Panel1.Show();
            ContainerAddDisplay.Panel2.Show();
            ContainerAddDisplay.Panel1.Hide();
            ContainerAll.SplitterDistance = ContainerAll.Width / 2;
            ContainerAddDisplay.SplitterDistance = 0;
            ContainerEditDelete.Panel2.Show();
            ContainerEditDelete.Panel1.Hide();
            ContainerEditDelete.SplitterDistance = 0;
            ReloadDelete();
        }

        // After a crop was selected and the button pressed we show a prompt to make sure this is the crop which they want to delete
        private void DeleteCropButton_Click(object sender, EventArgs e)
        {
            Prompt confirm = new(true);
            confirm.promptText.Text = "Are you sure you want to delete it?";
            confirm.ShowDialog();
            if (Prompt.Cancelled) return;
            Crop.DeleteCropWithName(Delete_CropToDeleteComboBox.Text);
            mChanged = true;
            ReloadDataGrid();
            ReloadDelete();
        }

        // Reloads the delete so we can make sure we are deleting correctly and not trying to delete and already deleted crop
        private void ReloadDelete()
        {
            Delete_CropToDeleteComboBox.Items.Clear();
            Delete_CropToDeleteComboBox.Items.AddRange(Crop.GetCropNames());
        }
    }
}
