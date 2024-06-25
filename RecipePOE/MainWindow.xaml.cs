using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipePOE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //List to hold all recipes
        public List<Recipe> recipes;
        public MainWindow()
        {
            InitializeComponent();
            recipes = new List<Recipe>();
            LoadInformation();
        }

        // Method to load and display calorie and food group information
        private void LoadInformation()
        {
            StringBuilder infoText = new StringBuilder();
            infoText.AppendLine("Calories Information:");
            infoText.AppendLine("Calories are a measure of the amount of energy in food. The average daily intake for adults is around 2000-2500 calories.");
            infoText.AppendLine("\nFood Groups Information:");
            infoText.AppendLine("1. Vegetables: Rich in vitamins, minerals, and fiber. Examples: spinach, carrots, broccoli.");
            infoText.AppendLine("2. Fruits: High in vitamins, fiber, and natural sugars. Examples: apples, bananas, berries.");
            infoText.AppendLine("3. Grains: Good source of carbohydrates and fiber. Examples: rice, oats, wheat.");
            infoText.AppendLine("4. Protein: Essential for building and repairing tissues. Examples: meat, beans, nuts.");
            infoText.AppendLine("5. Dairy: High in calcium and protein. Examples: milk, cheese, yogurt.");
            infoText.AppendLine("6. Carbohydrates: Primary source of energy. Examples: bread, pasta, potatoes.");
            infoText.AppendLine("7. Fats and Oils: Needed in small amounts for energy and cell functions. Examples: butter, olive oil, avocado.");

            // Display the information in the InformationTextBox
            InformationTextBox.Text = infoText.ToString();
        }

        // Event handler for AddIngredientsButton click event
        public void AddIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            int numOfIngredients;
            // Try to parse the number of ingredients from the textbox
            if (int.TryParse(NumberOfIngredientsTextBox.Text, out numOfIngredients))
            {
                IngredientsPanel.Children.Clear(); // Clear existing ingredients input fields

                // Loop to create input fields for each ingredient
                for (int i = 0; i < numOfIngredients; i++)
                {
                    StackPanel ingredientPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };

                    // Create textboxes and comboboxes for ingredient details
                    TextBox ingredientNameTextBox = new TextBox { Width = 100, Margin = new Thickness(5) };
                    TextBox ingredientQuantityTextBox = new TextBox { Width = 50, Margin = new Thickness(5) };
                    ComboBox ingredientUnitComboBox = new ComboBox { Width = 75, Margin = new Thickness(5) };
                    ingredientUnitComboBox.Items.Add("grams");
                    ingredientUnitComboBox.Items.Add("ml");
                    ingredientUnitComboBox.Items.Add("cups");
                    ingredientUnitComboBox.Items.Add("tbsp");
                    ingredientUnitComboBox.Items.Add("tsp");
                    ingredientUnitComboBox.Items.Add("l");

                    TextBox ingredientCaloriesTextBox = new TextBox { Width = 50, Margin = new Thickness(5) };
                    ComboBox ingredientFoodGroupComboBox = new ComboBox { Width = 100, Margin = new Thickness(5) };
                    ingredientFoodGroupComboBox.Items.Add("Vegetables");
                    ingredientFoodGroupComboBox.Items.Add("Fruits");
                    ingredientFoodGroupComboBox.Items.Add("Grains");
                    ingredientFoodGroupComboBox.Items.Add("Protein");
                    ingredientFoodGroupComboBox.Items.Add("Dairy");
                    ingredientFoodGroupComboBox.Items.Add("Carbohydrates");
                    ingredientFoodGroupComboBox.Items.Add("Fats and Oils");

                    // Add textboxes and comboboxes to the ingredient panel
                    ingredientPanel.Children.Add(new TextBlock { Text = "Ingredient:", Margin = new Thickness(5) });
                    ingredientPanel.Children.Add(ingredientNameTextBox);
                    ingredientPanel.Children.Add(new TextBlock { Text = "Quantity:", Margin = new Thickness(5) });
                    ingredientPanel.Children.Add(ingredientQuantityTextBox);
                    ingredientPanel.Children.Add(new TextBlock { Text = "Unit:", Margin = new Thickness(5) });
                    ingredientPanel.Children.Add(ingredientUnitComboBox);
                    ingredientPanel.Children.Add(new TextBlock { Text = "Calories:", Margin = new Thickness(5) });
                    ingredientPanel.Children.Add(ingredientCaloriesTextBox);
                    ingredientPanel.Children.Add(new TextBlock { Text = "Food Group:", Margin = new Thickness(5) });
                    ingredientPanel.Children.Add(ingredientFoodGroupComboBox);

                    // Add the ingredient panel to the main ingredients panel
                    IngredientsPanel.Children.Add(ingredientPanel);
                }
            }
            else
            {
                // Show error message if the input is not a valid number
                MessageBox.Show("Please enter a valid number of ingredients.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for SaveRecipeButton click event
        public void SaveRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Ensure the recipe name is not empty
                if (string.IsNullOrWhiteSpace(RecipeNameTextBox.Text))
                {
                    throw new Exception("Recipe name cannot be empty.");
                }

                // Create a new Recipe object
                Recipe recipe = new Recipe
                {
                    Name = RecipeNameTextBox.Text,
                    Ingredients = new List<Ingredient>(),
                    Steps = new List<string>(StepsTextBox.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                };

                int totalCalories = 0;

                // Loop through each ingredient panel and retrieve the ingredient details
                foreach (StackPanel panel in IngredientsPanel.Children)
                {
                    string ingredient = ((TextBox)panel.Children[1]).Text;
                    string quantity = ((TextBox)panel.Children[3]).Text;
                    string unit = ((ComboBox)panel.Children[5]).SelectedItem?.ToString();
                    string caloriesText = ((TextBox)panel.Children[7]).Text;
                    string foodGroup = ((ComboBox)panel.Children[9]).SelectedItem?.ToString();

                    // Ensure all ingredient fields are filled
                    if (string.IsNullOrWhiteSpace(ingredient) || string.IsNullOrWhiteSpace(quantity) || string.IsNullOrWhiteSpace(unit) || string.IsNullOrWhiteSpace(caloriesText) || string.IsNullOrWhiteSpace(foodGroup))
                    {
                        throw new Exception("All ingredient fields must be filled.");
                    }

                    // Ensure the quantity is a valid number
                    if (!double.TryParse(quantity, out double parsedQuantity))
                    {
                        throw new Exception("Quantity must be a number.");
                    }

                    // Ensure the calories is a valid number
                    if (!int.TryParse(caloriesText, out int calories))
                    {
                        throw new Exception("Calories must be a number.");
                    }

                    totalCalories += calories;

                    // Add the ingredient to the recipe
                    recipe.Ingredients.Add(new Ingredient
                    {
                        Name = ingredient,
                        Quantity = parsedQuantity.ToString(),
                        Unit = unit,
                        Calories = calories.ToString(),
                        FoodGroup = foodGroup,
                    });
                }

                // Show a warning if the total calories exceed 300
                if (totalCalories > 300)
                {
                    MessageBox.Show("Warning: Total calories exceed 300.", "Calorie Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                // Add the recipe to the list and update the list box
                recipes.Add(recipe);
                UpdateRecipesListBox(recipes);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to clear the form inputs
        public void ClearForm()
        {
            RecipeNameTextBox.Clear();
            NumberOfIngredientsTextBox.Clear();
            IngredientsPanel.Children.Clear();
            StepsTextBox.Clear();
        }

        // Method to update the list box with given recipes
        public void UpdateRecipesListBox(IEnumerable<Recipe> recipesToDisplay)
        {
            RecipesListBox.Items.Clear();
            foreach (var recipe in recipesToDisplay.OrderBy(r => r.Name))
            {
                RecipesListBox.Items.Add(recipe.Name);
            }
        }

        // Event handler for RecipesListBox selection change event
        public void RecipesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipesListBox.SelectedItem != null)
            {
                string selectedRecipeName = RecipesListBox.SelectedItem.ToString();
                Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name == selectedRecipeName);
                if (selectedRecipe != null)
                {
                    DisplayRecipe(selectedRecipe);
                }
            }
        }

        // Method to display the selected recipe details
        public void DisplayRecipe(Recipe recipe)
        {
            StringBuilder recipeDetails = new StringBuilder();
            recipeDetails.AppendLine($"Recipe Name: {recipe.Name}");
            recipeDetails.AppendLine("Ingredients:");

            // Loop through each ingredient and add its details to the string builder
            foreach (var ingredient in recipe.Ingredients)
            {
                double originalQuantity = double.Parse(ingredient.Quantity);
                recipeDetails.AppendLine($"- {ingredient.Name}: {ingredient.Quantity} {ingredient.Unit}, {ingredient.Calories} calories, Food Group: {ingredient.FoodGroup}");
            }

            recipeDetails.AppendLine("\nSteps:");
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                recipeDetails.AppendLine($"{i + 1}. {recipe.Steps[i]}");
            }

            // Display the recipe details in the RecipeDisplayTextBlock
            RecipeDisplayTextBlock.Text = recipeDetails.ToString();
        }

        // Event handler for ApplyFilterButton click event
        public void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string filterIngredient = FilterIngredientTextBox.Text.ToLower();
            string filterFoodGroup = ((ComboBoxItem)FilterFoodGroupComboBox.SelectedItem).Content.ToString();
            string filterMaxCaloriesText = FilterMaxCaloriesTextBox.Text;
            int filterMaxCalories = string.IsNullOrWhiteSpace(filterMaxCaloriesText) ? int.MaxValue : int.Parse(filterMaxCaloriesText);

            // Filter recipes based on the filter criteria
            var filteredRecipes = recipes.Where(recipe =>
                (string.IsNullOrWhiteSpace(filterIngredient) || recipe.Ingredients.Any(i => i.Name.ToLower().Contains(filterIngredient))) &&
                (filterFoodGroup == "Any" || recipe.Ingredients.Any(i => i.FoodGroup == filterFoodGroup)) &&
                (recipe.Ingredients.Sum(i => int.Parse(i.Calories)) <= filterMaxCalories)
            ).ToList();
            foreach (var recipe in filteredRecipes)
            {
                MessageBox.Show("Recipe Name: " + recipe.Name + "\n");
            }

            UpdateRecipesListBox(filteredRecipes);
        }

        // Event handler for ClearFormButton click event
        public void ClearFormButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }
    }
}
    