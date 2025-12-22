using System;
using VetClinic;

namespace VetClinicConsole {
    public class AppNavigator {
        private Database _database;
        private Window _currentWindow;

        public AppNavigator(Database db) {
            _database = db;
            _currentWindow = new AlmostOpusMagnum();
        }

        public void Start() {
            while (_currentWindow != null) {
                try {
                    Window nextWindow = _currentWindow.Run(_database);
                    _currentWindow = nextWindow;
                } catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                    _currentWindow = new AlmostOpusMagnum();
                }
            }

            Console.WriteLine("Application closed.");
        }
    }
}