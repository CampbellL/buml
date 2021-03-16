using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace BUML.GUI.Views
{
    public class MainWindow : Window
    {
        public static readonly BindingFlags SearchFlags = BindingFlags.NonPublic |
                                                          BindingFlags.Instance |
                                                          BindingFlags.Public |
                                                          BindingFlags.Static |
                                                          BindingFlags.DeclaredOnly;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private static void GenerateUml(Assembly assembly)
        {
            using (StreamWriter writer = new("uml.md"))
            {
                StringBuilder inheritanceBuilder = new();
                Type[]? types = assembly.GetTypes();
                //writer.WriteLine("```mermaid");
                writer.WriteLine("classDiagram");
                foreach (var type in types)
                {
                    writer.WriteLine($"  class {type.Name} {{");
                    foreach (FieldInfo field in type.GetFields(SearchFlags))
                    {
                        string accessibility =
                            field.IsPublic ? "+" : field.IsPrivate ? "-" : "#";
                        writer.WriteLine(
                            $"     {accessibility}{field.FieldType.Name} {field.Name}");
                    }

                    foreach (var method in type.GetMethods(SearchFlags))
                    {
                        string accessibility =
                            method.IsPublic ? "+" : method.IsPrivate ? "-" : "#";
                        writer.WriteLine($"     {accessibility}{method.Name}() {method.ReturnType.Name}");
                    }

                    foreach (var implementedInterface in type.GetInterfaces())
                    {
                        inheritanceBuilder.AppendLine($"{implementedInterface.Name} <.. {type.Name}");
                    }

                    writer.WriteLine("  }");
                    if (type.BaseType != null && type.BaseType != typeof(object))
                    {
                        inheritanceBuilder.AppendLine($"{type.BaseType.Name} <|-- {type.Name}");
                    }
                }
                writer.WriteLine(inheritanceBuilder.ToString());
            }
            Debug.WriteLine(File.ReadAllText("uml.md"));
            
            if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                BashUtility.RunCommand("mmdc -w 4096 -h 2048 -i uml.md -o test.png");
        }

        private async void MenuItem_OnClick(object? sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog(){AllowMultiple = false};
            string[]? path = await dialog.ShowAsync(this);
            Assembly? assembly = Assembly.LoadFile(path[0]);
            GenerateUml(assembly);
            var image = this.Get<Image>("PreviewImage");
            image.Source = new Bitmap(File.Open("test.png", FileMode.Open));
        }
    }
}