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
        private readonly int pRows = 9, pCols = 1;

        private readonly Image[,] gridImages;
        private readonly Image[,] panelImages;

        private EnergyState energyState;
        private PanelState panelState;

     /*   private Position selectedPos = null;
        private PositionPanel PositionPanel = null;
*/
        public MainWindow()
        {
            InitializeComponent();
            panelImages = SetupPanel();
            gridImages = SetupLand();
            energyState = new EnergyState(rows, cols);
            panelState = new PanelState(pRows, pCols);
            DrawPanelWithStructures();
            DrawGridWithStructures();

        }

        // Grid Stuff----------------------------------------------------------------
        private Image[,] SetupLand()
        {
            /// Creates an array of images the same size
            /// of the board and sets the UniformGrid that
            /// is named "GameGrid" rows and columns equal
            /// to the ones here.
            Image[,] images = new Image[rows, cols];

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

        private Position ToSquarePosition(Point point)
        {
            double squaresize = GridBorder.ActualWidth / 20;
            int row = (int)(point.Y / squaresize);
            int col = (int)(point.X / squaresize);
            return new Position(row, col);
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

            ImageSource selectedImage = GetImageAtPositionOnPanel(pospan);

            if (selectedImage != null)
            {
                MessageBox.Show($"Selected position - Row: {pospan.Row}, Column: {pospan.Column}\nImage: {selectedImage}");
            }
            else
            {
                MessageBox.Show($"No image found at position - Row: {pospan.Row}, Column: {pospan.Column}");
            }
        }

        private ImageSource GetImageAtPositionOnPanel(PositionPanel pospan)
        {
            if (pospan.Row < 0 || pospan.Row >= pRows || pospan.Column < 0 || pospan.Column >= pCols)
            {
                return null;
            }

            Image selectedImage = panelImages[pospan.Row, pospan.Column];
            return selectedImage.Source;
        }

        private PositionPanel ToSquarePositionPanel(Point point)
        {
            double squaresize = SidePanelBorder.ActualWidth / pRows;
            int row = (int)(point.Y / squaresize);
            int col = (int)(point.X / squaresize);
            return new PositionPanel(row, col);
        }

        private void DrawPanelWithStructures()
        {
            for (int r = 0; r < pRows; r++)
            {
                LandValue panVal = panelState.Panel[r, 0];
                panelImages[r, 0].Source = gridValToImage[panVal];
            }
        }
    }
}
