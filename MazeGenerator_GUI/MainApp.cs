using System.Windows.Forms;

namespace MazeGenerator_GUI {
    class MainApp {
        static void Main(string[] args) {
            MazeGUI mazeGUI = new MazeGUI();
            Application.Run(mazeGUI);
        }
    }
}
