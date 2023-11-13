using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EnergySim
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<LandValue, ImageSource> gridValToImage = new()
        {
            {LandValue.Empty, Images.Empty},
            {LandValue.Nuclear, Images.Nuclear},
            {LandValue.Turbine, Images.Turbine },
            {LandValue.House, Images.House},
            {LandValue.Business, Images.Business},
            {LandValue.Geothermal, Images.Geothermal},
            {LandValue.Hydroelectric, Images.Hydroelectric},
            {LandValue.Solar, Images.Solar},
            {LandValue.Biomass, Images.Biomass}    
        };

        private readonly int rows = 20, cols = 20;
        private readonly int pRows = 10, pCols = 1;

        private readonly Image[,] gridImages;
        private readonly Image[,] panelImages;

        private EnergyState energyState;
        private PanelState panelState;

        public MainWindow()
        {
            InitializeComponent();
            panelImages = SetupPanel();
            panelState = new PanelState(pRows, pCols);

            gridImages = SetupLand();
            energyState = new EnergyState(rows, cols);

            DrawPanelWithStructures();
            DrawGridWithStructures();
            

        }

        // Grid Stuff----------------------------------------------------------------
        private Image[,] SetupLand()
        {
            Image[,] images = new Image[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        Tag = LandValue.Empty // Associate LandValue with Image
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }

        private void GridBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(GridBorder);
            Position sourcePos = ToSquarePosition(point);

            LandValue selectedLandValue = GetLandValueAtPosition(sourcePos);

            if (selectedLandValue != LandValue.Empty)
            {
                /*MessageBox.Show($"Selected position in Grid - Row: {sourcePos.Row}, Column: {sourcePos.Column}\nLandValue: {selectedLandValue}");*/

                // Move the selected structure to a new position in the grid
                MoveStructureInGrid(sourcePos, selectedLandValue);
            }
            else
            {
                MessageBox.Show($"No LandValue found at position in Grid - Row: {sourcePos.Row}, Column: {sourcePos.Column}");
            }
        }

        private void MoveStructureInGrid(Position sourcePos, LandValue selectedLandValue)
        {
            // Find an empty cell in the grid for the destination position
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (energyState.LandGrid[r, c] == LandValue.Empty)
                    {
                        // Move the structure from source position to the destination position
                        energyState.LandGrid[r, c] = selectedLandValue;
                        gridImages[r, c].Source = gridValToImage[selectedLandValue];
                        gridImages[r, c].Tag = selectedLandValue; // Associate LandValue with Image

                        // Clear the structure from the source position
                        energyState.LandGrid[sourcePos.Row, sourcePos.Column] = LandValue.Empty;
                        gridImages[sourcePos.Row, sourcePos.Column].Source = gridValToImage[LandValue.Empty];
                        gridImages[sourcePos.Row, sourcePos.Column].Tag = LandValue.Empty; // Clear the LandValue association

                        return; // Exit the loop after moving the structure
                    }
                }
            }

            // Handle the case where there is no empty cell in the grid for the destination position
            MessageBox.Show("No empty cell in the grid to move the structure.");
        }

        private Position ToSquarePosition(Point point)
        {
            double squaresize = GridBorder.ActualWidth / 20;
            int row = (int)(point.Y / squaresize);
            int col = (int)(point.X / squaresize);
            return new Position(row, col);
        }

        private LandValue GetLandValueAtPosition(Position pos)
        {
            if (pos.Row < 0 || pos.Row >= rows || pos.Column < 0 || pos.Column >= cols)
            {
                return LandValue.Empty;
            }

            Image selectedImage = gridImages[pos.Row, pos.Column];
            return (LandValue)selectedImage.Tag;
        }

        private void DrawGridWithStructures()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    LandValue landVal = energyState.LandGrid[r, c];
                    Image gridImage = gridImages[r, c];

                    gridImage.Source = gridValToImage[landVal];
                    gridImage.Tag = landVal; 
                }
            }
        }

        // Create a Move to Position
        // Execute move

        // Panel Stuff----------------------------------------------------------------
        private Image[,] SetupPanel()
        {
            Image[,] panelImage = new Image[pRows, pCols];
            SidePanel.Rows = pRows;
            SidePanel.Columns = pCols;
            for (int r = 0; r < pRows; r++)
            {
                Image structure = new Image
                {
                    Source = Images.Empty,
                    Tag = LandValue.Empty // Associate LandValue with Image
                };
                panelImage[r, 0] = structure;
                SidePanel.Children.Add(structure);
            }
            return panelImage;
        }

        private void SidePanelBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(SidePanelBorder);
            PositionPanel pospan = ToSquarePositionPanel(point);

            LandValue selectedLandValue = GetLandValueAtPositionOnPanel(pospan);

            if (selectedLandValue != LandValue.Empty)
            {
                

                // Add the selected structure to the grid
                AddStructureToGrid(selectedLandValue);
            }
            else
            {
                MessageBox.Show($"No LandValue found at position in SidePanel - Row: {pospan.Row}, Column: {pospan.Column}");
            }
        }

        private void AddStructureToGrid(LandValue selectedLandValue)
        {
            // Find an empty cell in the grid and add the structure
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (energyState.LandGrid[r, c] == LandValue.Empty)
                    {
                        energyState.LandGrid[r, c] = selectedLandValue;
                        gridImages[r, c].Source = gridValToImage[selectedLandValue];
                        gridImages[r, c].Tag = selectedLandValue; // Associate LandValue with Image
                        return; // Exit the loop after adding the structure
                    }
                }
            }
        }

        private LandValue GetLandValueAtPositionOnPanel(PositionPanel pospan)
        {
            if (pospan.Row < 0 || pospan.Row >= pRows || pospan.Column < 0 || pospan.Column >= pCols)
            {
                return LandValue.Empty; // Return a default value or handle out-of-bounds accordingly
            }

            Image selectedImage = panelImages[pospan.Row, pospan.Column];
            return (LandValue)selectedImage.Tag;
        }


        private PositionPanel ToSquarePositionPanel(Point point)
        {
            double squaresize = 500 / pRows;
            int prow = (int)(point.Y/ squaresize);
            int pcol = (int)(point.X/ squaresize);
            return new PositionPanel(prow, pcol);
        }

        private void DrawPanelWithStructures()
        {
            for (int r = 0; r < pRows; r++)
            {
                LandValue panVal = panelState.Panel[r, 0];
                Image panelImage = panelImages[r, 0];

                panelImage.Source = gridValToImage[panVal];
                panelImage.Tag = panVal; // Associate LandValue with Image
            }
        }
    }
}
