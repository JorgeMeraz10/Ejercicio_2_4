using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ejercicio2_4.Models;
using Ejercicio2_4.Helpers;
using SQLite;
using SignaturePad.Forms;

namespace Ejercicio2_4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private SQLiteHelper _databaseHelper;


        private ObservableCollection<Signatures> _firmasList;

        public MainPage()
        {
            InitializeComponent();
            _databaseHelper = new SQLiteHelper();

            // Crear una colección observable para almacenar las firmas
            _firmasList = new ObservableCollection<Signatures>();
            firmasListView.ItemsSource = _firmasList; // Asignar la colección al ListView
            CargarFirmasGuardadas(); // Cargar las firmas guardadas desde la base de datos
        }

        private async void GuardarButton_Clicked(object sender, EventArgs e)
        {
            // Capturar la firma como una imagen
            var signatureImage = await signaturePadView.GetImageStreamAsync(SignatureImageFormat.Png);

            // Convertir la imagen a un array de bytes (BLOB)
            byte[] signatureBytes;
            using (var memoryStream = new MemoryStream())
            {
                await signatureImage.CopyToAsync(memoryStream);
                signatureBytes = memoryStream.ToArray();
            }

            // Crear una nueva instancia de la clase Signatures con los datos a guardar
            var signature = new Signatures
            {
                Descripcion = descripcionEntry.Text,
                FirmaDigital = signatureBytes
            };

            // Obtener la conexión a la base de datos
            // Guardar la firma en la base de datos
            using (var db = _databaseHelper.GetConnection())
            {
                db.Insert(signature);
            }

            // Agregar la firma a la lista y actualizar el ListView
            _firmasList.Add(signature);

            // Mostrar una alerta de éxito
            await DisplayAlert("¡Éxito!", "La firma se ha guardado correctamente.", "OK");
        }

        private void CargarFirmasGuardadas()
        {
            // Limpiar la lista antes de cargar las firmas guardadas
            _firmasList.Clear();

            // Obtener las firmas guardadas desde la base de datos
            using (var db = _databaseHelper.GetConnection())
            {
                var firmasGuardadas = db.Table<Signatures>().ToList();
                foreach (var firma in firmasGuardadas)
                {
                    _firmasList.Add(firma);
                }
            }
        }
    }
}
