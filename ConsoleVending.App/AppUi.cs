using Terminal.Gui;

namespace ConsoleVending.App
{
    public class AppUi {
        public void Init(){
            Application.Init();

            var menu = new MenuBar (new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Quit", "quit app", () => {
                        if(Quit()) Application.RequestStop();
                    })
                })
            });

            var win = new Window ("Hello") {
                X = 0,
                Y = 1,
                Width = Dim.Fill (),
                Height = Dim.Fill () - 1
            };

            Application.Top.Add(menu, win);
            Application.Run();
        }

        private bool Quit(){
            var n = MessageBox.Query("Quit", "Are you sure you want to leave the vending machine?", "Yes", "No");
            return n == 0;
        }
    }
}