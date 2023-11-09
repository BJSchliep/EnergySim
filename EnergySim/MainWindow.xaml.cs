using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        private readonly Image[,] gridImages;
        private readonly Image[,] panelImages;
        private EnergyState energyState;
        private Position selectedPos = null;

        public MainWindow()
        {
            InitializeComponent();
            panelImages = SetupPanel();
            gridImages = SetupLand();
            energyState = new EnergyState(rows, cols);
            DrawGridWithStructures();

        }

        private Image[,] SetupLand()
        {
            /// Creates an array of images the same size
            /// of the board and sets the UniformGrid that
            /// is named "GameGrid" rows and columns equal
            /// to the ones here.
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }

        private Image[,] SetupPanel()
        {
            Image[,] panelImage = new Image[9, 1];
            SidePanel.Rows = 9;
            SidePanel.Columns = 1;

            ImageSource[] images = new ImageSource[]
            {
                Images.Biomass,
                Images.Geothermal,
                Images.Hydroelectric,
                Images.Nuclear,
                Images.Solar,
                Images.Turbine,
                Images.Empty,
                Images.Business,
                Images.House
            };

            for (int r = 0; r < 9; r++)
            {
                Image structure = new Image
                {
                    Source = images[r]
                };

                panelImage[r, 0] = structure;
                SidePanel.Children.Add(structure);
            }
            return panelImage;
        }


        // Working on this -------------------------------------
        private void GridBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(GridBorder);
            Position pos = ToSquarePosition(point);

            ImageSource selectedImage = GetImageAtPosition(pos);

            if (selectedImage != null)
            {
                MessageBox.Show($"Selected position - Row: {pos.Row}, Column: {pos.Column}\nImage: {selectedImage}");
            }
            else
            {
                MessageBox.Show($"No image found at position - Row: {pos.Row}, Column: {pos.Column}");
            }
        }

        private void SidePanelBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(SidePanelBorder);
            Position pos = ToSquarePosition(point);

            ImageSource selectedImage = GetImageAtPosition(pos);

            if (selectedImage != null)
            {
                MessageBox.Show($"Selected position - Row: {pos.Row}, Column: {pos.Column}\nImage: {selectedImage}");
            }
            else
            {
                MessageBox.Show($"No image found at position - Row: {pos.Row}, Column: {pos.Column}");
            }
        }

        private ImageSource GetImageAtPosition(Position pos)
        {
            if (pos.Row < 0 || pos.Row >= rows || pos.Column < 0 || pos.Column >= cols)
            {
                return null;
            }

            Image selectedImage = gridImages[pos.Row, pos.Column];
            return selectedImage.Source;
        }


        private Position ToSquarePosition(Point point)
        {
            double squaresize = GridBorder.ActualWidth / 20;
            int row = (int)(point.Y / squaresize);
            int col = (int)(point.X / squaresize);
            return new Position(row, col);
        }

        
        

        // -----------------------------------------------------
        private void DrawGridWithStructures()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    LandValue landVal = energyState.LandGrid[r, c];
                    gridImages[r, c].Source = gridValToImage[landVal];
                }
            }
        }


        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= rows || pos.Column < 0 || pos.Column >= cols;
        }
    }
}
