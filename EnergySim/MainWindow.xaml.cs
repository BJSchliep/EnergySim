using System.Collections.Generic;
using System.Security.Policy;
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
            {LandValue.Biomass, Images.Biomass},
            {LandValue.Water, Images.Water }
        };

        private readonly int rows = 20, cols = 20;
        private readonly int pRows = 10, pCols = 1;

        private readonly Image[,] gridImages;
        private readonly Image[,] panelImages;

        private readonly EnergyState energyState;
        private readonly PanelState panelState;
        private readonly Money Money;

        private Position firstClickPosition;
        private LandValue selectedLandValue;


        public MainWindow()
        {
            InitializeComponent();
            panelImages = SetupPanel();
            panelState = new PanelState(pRows, pCols);

            gridImages = SetupLand();
            energyState = new EnergyState(rows, cols);

            
            Money = new Money(60);
            DisplayMoney();

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
                    Image image = new()
                    {
                        Source = Images.Empty,
                        Tag = LandValue.Empty 
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
            Position clickedPosition = ToSquarePosition(point);

            if (firstClickPosition == null)
            {
                selectedLandValue = GetLandValueAtPosition(clickedPosition);

                if (selectedLandValue != LandValue.Empty)
                {
                    firstClickPosition = clickedPosition;
                }
                else
                {
                    MessageBox.Show($"No LandValue found at position in Grid - Row: {clickedPosition.Row}, Column: {clickedPosition.Column}");
                }
            }
            else
            {
                if (firstClickPosition.Equals(clickedPosition))
                {
                    RemoveStructureFromGrid(clickedPosition, selectedLandValue);
                }
                else
                {
                    MoveStructureInGrid(firstClickPosition, clickedPosition, selectedLandValue);
                }

                firstClickPosition = null;
                selectedLandValue = LandValue.Empty;
            }
        }

        private void RemoveStructureFromGrid(Position position, LandValue landValue)
        {
            energyState.LandGrid[position.Row, position.Column] = LandValue.Empty;
            gridImages[position.Row, position.Column].Source = gridValToImage[LandValue.Empty];
            gridImages[position.Row, position.Column].Tag = LandValue.Empty;

            // Use the SubtractMoney method from the Money class
            Money.SubtractMoney(landValue);
            DisplayMoney();
        }

        private void MoveStructureInGrid(Position sourcePos, Position destPos, LandValue selectedLandValue)
        {
            if (energyState.LandGrid[destPos.Row, destPos.Column] == LandValue.Empty)
            {
                energyState.LandGrid[destPos.Row, destPos.Column] = selectedLandValue;
                gridImages[destPos.Row, destPos.Column].Source = gridValToImage[selectedLandValue];
                gridImages[destPos.Row, destPos.Column].Tag = selectedLandValue; 

                energyState.LandGrid[sourcePos.Row, sourcePos.Column] = LandValue.Empty;
                gridImages[sourcePos.Row, sourcePos.Column].Source = gridValToImage[LandValue.Empty];
                gridImages[sourcePos.Row, sourcePos.Column].Tag = LandValue.Empty; 
            }
            else
            {
                MessageBox.Show("Cannot move to a non-empty cell in the grid.");
            }
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

        // Panel Stuff----------------------------------------------------------------
        private Image[,] SetupPanel()
        {
            Image[,] panelImage = new Image[pRows, pCols];
            SidePanel.Rows = pRows;
            SidePanel.Columns = pCols;
            for (int r = 0; r < pRows; r++)
            {
                Image structure = new()
                {
                    Source = Images.Empty
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
                AddStructureToGrid(selectedLandValue);
            }
            else
            {
                MessageBox.Show($"No LandValue found at position in SidePanel - Row: {pospan.Row}, Column: {pospan.Column}");
            }
        }

        private void AddStructureToGrid(LandValue selectedLandValue)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (energyState.LandGrid[r, c] == LandValue.Empty)
                    {
                        Money.SubtractMoney(selectedLandValue);
                        energyState.LandGrid[r, c] = selectedLandValue;
                        gridImages[r, c].Source = gridValToImage[selectedLandValue];
                        gridImages[r, c].Tag = selectedLandValue;

                        return;
                    }
                }
            }
        }

        private LandValue GetLandValueAtPositionOnPanel(PositionPanel pospan)
        {
            if (pospan.Row < 0 || pospan.Row >= pRows || pospan.Column < 0 || pospan.Column >= pCols)
            {
                return LandValue.Empty; 
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
                panelImage.Tag = panVal;
            }
        }

        // Money Stuff
        private void DisplayMoney()
        {
            MoneyText.Text = $"Money: {Money.GetTotalMoney()}";
        }
    }
}
