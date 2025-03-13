using ProyectoSano.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ProyectoSano.Pages;

public partial class AdminPage : ContentPage
{
    private List<Entry> _ingredientesEntries = new List<Entry>();
    private List<Entry> _preparacionEntries = new List<Entry>();
    private FileResult _selectedRecetaImageFile;
    private FileResult _selectedProductoImageFile;
    private FileResult _image1Rutina;
    private FileResult _image2Rutina;


    public AdminPage()
    {
        InitializeComponent();
    }
    private void OnCategoryTapped(object sender, EventArgs e)
    {
        foreach (var child in category.Children)
        {
            if (child is Label label)
            {
                label.BackgroundColor = Colors.Transparent;
                label.TextColor = Color.FromHex("#ec268f");
            }
        }

        if (sender is Label tappedLabel)
        {
            RecetaForm.IsVisible = false;
            recetaBorder.IsVisible = false;
            AlimentacionForm.IsVisible = false;
            alimentacionBorder.IsVisible = false;
            ProductoForm.IsVisible = false;
            productoBorder.IsVisible = false;
            VideoForm.IsVisible = false;
            videoBorder.IsVisible = false;
            RutinaForm.IsVisible = false;
            rutinaBorder.IsVisible = false;

            switch (tappedLabel.Text)
            {
                case "Receta":
                    recetaBorder.IsVisible = true;
                    RecetaForm.IsVisible = true;
                    break;
                case "Alimenctacion":
                    alimentacionBorder.IsVisible = true;
                    AlimentacionForm.IsVisible = true;
                    break;
                case "Producto":
                    productoBorder.IsVisible = true;
                    ProductoForm.IsVisible = true;
                    break;
                case "Video":
                    videoBorder.IsVisible = true;
                    VideoForm.IsVisible = true;
                    break;
                case "Rutina":
                    rutinaBorder.IsVisible = true;
                    RutinaForm.IsVisible = true;
                    break;
            }

            tappedLabel.BackgroundColor = Colors.White;
            tappedLabel.TextColor = Color.FromHex("#a8cf45");
        }
    }

    private void OnIngredienteCountChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(IngredienteCountEntry.Text, out int count))
        {
            _ingredientesEntries.Clear();
            IngredientesStack.Children.Clear();

            for (int i = 0; i < count; i++)
            {
                var entry = new Entry { Placeholder = $"Ingrediente {i + 1}" };
                _ingredientesEntries.Add(entry);
                IngredientesStack.Children.Add(entry);
            }
        }
    }

    private void OnPreparacionCountChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(PreparacionCountEntry.Text, out int count))
        {
            _preparacionEntries.Clear();
            PreparacionStack.Children.Clear();

            for (int i = 0; i < count; i++)
            {
                var entry = new Entry { Placeholder = $"Paso {i + 1}" };
                _preparacionEntries.Add(entry);
                PreparacionStack.Children.Add(entry);
            }
        }
    }

    private async void OnSelectRecetaImageClicked(object sender, EventArgs e)
    {
        await SelectImage(false);
    }

    private async void OnSelectProductoImageClicked(object sender, EventArgs e)
    {
        await SelectImage(true);
    }

    private async Task SelectImage(bool isProducto)
    {
        var selectedImageFile = await FilePicker.Default.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Seleccione una imagen"
        });

        if (selectedImageFile != null)
        {
            var stream = await selectedImageFile.OpenReadAsync();

            if (isProducto)
            {
                _selectedProductoImageFile = selectedImageFile;
                ProductoSelectedImage.Source = ImageSource.FromStream(() => stream); // Asegúrate de que `ProductoSelectedImage` esté en el XAML
            }
            else
            {
                _selectedRecetaImageFile = selectedImageFile;
                SelectedImage.Source = ImageSource.FromStream(() => stream); // Este es para Receta
            }
        }
    }

    private async Task<string> UploadImageAsync(FileResult fileResult)
    {
        using var stream = await fileResult.OpenReadAsync();
        var content = new MultipartFormDataContent
            {
                { new StreamContent(stream), "image", fileResult.FileName }
            };

        var response = await new HttpClient().PostAsync("http://nekjoesg-001-site5.anytempurl.com/api/Image/UploadImage", content);
        if (response.IsSuccessStatusCode)
        {
            var jsonElement = await response.Content.ReadFromJsonAsync<JsonElement>();

            if (jsonElement.TryGetProperty("imageUrl", out JsonElement imageUrlElement))
            {
                return imageUrlElement.GetString();
            }
        }
        return null;
    }

    private async void OnSaveRecetaClicked(object sender, EventArgs e)
    {
        string _imageUrl = "";

        if (_selectedRecetaImageFile != null)
        {
            _imageUrl = await UploadImageAsync(_selectedRecetaImageFile);
        }

        var receta = new Receta
        {
            Nombre = NombreEntry.Text,
            Subtitulo = SubtituloEntry.Text,
            Ingredientes = _ingredientesEntries.ConvertAll(entry => entry.Text).ToArray(),
            Preparacion = _preparacionEntries.ConvertAll(entry => entry.Text).ToArray(),
            Categoria = (string)CategoriaPicker.SelectedItem,
            Imagen = _imageUrl
        };

        var json = JsonSerializer.Serialize(receta);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await new HttpClient().PostAsync("http://nekjoesg-001-site5.anytempurl.com/api/Receta", content);
        if (response.IsSuccessStatusCode)
        {
            ClearRecetaForm();
            await DisplayAlert("Éxito", "Receta guardada correctamente.", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Ocurrió un error al guardar la receta.", "OK");
        }
    }

    private void ClearRecetaForm()
    {
        NombreEntry.Text = string.Empty;
        SubtituloEntry.Text = string.Empty;
        IngredienteCountEntry.Text = string.Empty;
        PreparacionCountEntry.Text = string.Empty;
        CategoriaPicker.SelectedItem = null;
        SelectedImage.Source = null;
        _selectedRecetaImageFile = null;

        _ingredientesEntries.Clear();
        IngredientesStack.Children.Clear();
        _preparacionEntries.Clear();
        PreparacionStack.Children.Clear();
    }


    private async void OnSaveAlimentacionClicked(object sender, EventArgs e)
    {
        var alimentacion = new Alimentacion
        {
            Titulo = TituloEntry.Text,
            Desayuno = DesayunoEntry.Text,
            Almuerzo = AlmuerzoEntry.Text,
            Cena = CenaEntry.Text,
            UserId = 1
        };

        var json = JsonSerializer.Serialize(alimentacion);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await new HttpClient().PostAsync("http://nekjoesg-001-site5.anytempurl.com/api/Alimentacion", content);
        if (response.IsSuccessStatusCode)
        {
            ClearAlimentacionForm();
            await DisplayAlert("Éxito", "Alimentación guardada correctamente.", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Ocurrió un error al guardar la alimentación.", "OK");
        }
    }

    private void ClearAlimentacionForm()
    {
        TituloEntry.Text = string.Empty;
        DesayunoEntry.Text = string.Empty;
        AlmuerzoEntry.Text = string.Empty;
        CenaEntry.Text = string.Empty;
        UsuarioPicker.SelectedIndex = -1;
    }


    private async void OnSaveProductoClicked(object sender, EventArgs e)
    {
        string imageUrl = "";

        if (_selectedProductoImageFile != null)
        {
            imageUrl = await UploadImageAsync(_selectedProductoImageFile);
        }

        var producto = new Productos
        {
            Titulo = ProductoTituloEntry.Text,
            Imagen = imageUrl,
            Precio = int.TryParse(ProductoPrecioEntry.Text, out int precio) ? precio : 0,
            Categoria = ProductoCategoriaPicker.SelectedItem?.ToString()
        };

        var response = await new HttpClient().PostAsJsonAsync("http://nekjoesg-001-site5.anytempurl.com/api/Producto", producto);
        if (response.IsSuccessStatusCode)
        {
            ClearProductoForm();
            await DisplayAlert("Éxito", "Producto creado correctamente", "OK");
        }
        else
        {
            await DisplayAlert("Error", "No se pudo crear el producto", "OK");
        }
    }

    private void ClearProductoForm()
    {
        ProductoTituloEntry.Text = string.Empty;
        ProductoPrecioEntry.Text = string.Empty;
        ProductoCategoriaPicker.SelectedIndex = -1;
        ProductoCategoriaPicker.SelectedItem = null;
        ProductoSelectedImage.Source = null;
        _selectedProductoImageFile = null;
    }


    private async void OnSaveVideoClicked(object sender, EventArgs e)
    {
        var video = new Videos
        {
            Titulo = VideoTituloEntry.Text,
            Image = "",
            Url = VideoUrlEntry.Text,
        };

        var response = await new HttpClient().PostAsJsonAsync("http://nekjoesg-001-site5.anytempurl.com/api/Video", video);
        if (response.IsSuccessStatusCode)
        {
            ClearVideoForm();
            await DisplayAlert("Éxito", "Video creado correctamente", "OK");
        }
        else
        {
            await DisplayAlert("Error", "No se pudo crear el video", "OK");
        }
    }

    private void ClearVideoForm()
    {
        VideoTituloEntry.Text = string.Empty;
        VideoUrlEntry.Text = string.Empty;
    }

    private void OnPosicionInicialCountCompleted(object sender, System.EventArgs e)
    {
        PosicionInicialStack.Children.Clear();
        if (int.TryParse(PosicionInicialCountEntry.Text, out int count))
        {
            for (int i = 0; i < count; i++)
            {
                Entry entry = new Entry { Placeholder = $"Posición {i + 1}" };
                PosicionInicialStack.Children.Add(entry);
            }
        }
    }

    private void OnMovimientosCountCompleted(object sender, System.EventArgs e)
    {
        MovimientosStack.Children.Clear();
        if (int.TryParse(MovimientosCountEntry.Text, out int count))
        {
            for (int i = 0; i < count; i++)
            {
                Entry entry = new Entry { Placeholder = $"Movimiento {i + 1}" };
                MovimientosStack.Children.Add(entry);
            }
        }
    }

    private void OnRespiracionesCountCompleted(object sender, System.EventArgs e)
    {
        RespiracionesStack.Children.Clear();
        if (int.TryParse(RespiracionesCountEntry.Text, out int count))
        {
            for (int i = 0; i < count; i++)
            {
                Entry entry = new Entry { Placeholder = $"Respiración {i + 1}" };
                RespiracionesStack.Children.Add(entry);
            }
        }
    }

    private void OnRepeticionesCountCompleted(object sender, System.EventArgs e)
    {
        RepeticionesStack.Children.Clear();
        if (int.TryParse(RepeticionesCountEntry.Text, out int count))
        {
            for (int i = 0; i < count; i++)
            {
                Entry entry = new Entry { Placeholder = $"Repetición {i + 1}" };
                RepeticionesStack.Children.Add(entry);
            }
        }
    }

    private async void OnSaveRutinaClicked(object sender, System.EventArgs e)
    {
        string imageUrl1 = "";
        string imageUrl2 = "";

        if (_image1Rutina != null && _image2Rutina != null)
        {
            imageUrl1 = await UploadImageAsync(_image1Rutina);
            imageUrl2 = await UploadImageAsync(_image2Rutina);
        }

        var rutina = new Rutina
        {
            Nombre = RutinaNombreEntry.Text,
            Titulo = RutinaTituloEntry.Text,
            Subtitulo = RutinaSubtituloEntry.Text,
            Dia = RutinaDiaPicker.SelectedItem.ToString(),
            UserId = 1,
            Image1 = imageUrl1,
            Image2 = imageUrl2,
        };

        rutina.PosicionInicial = PosicionInicialStack.Children
            .OfType<Entry>()
            .Select(entry => entry.Text)
            .ToArray();

        rutina.Movimientos = MovimientosStack.Children
            .OfType<Entry>()
            .Select(entry => entry.Text)
            .ToArray();

        rutina.Respiraciones = RespiracionesStack.Children
            .OfType<Entry>()
            .Select(entry => entry.Text)
            .ToArray();

        rutina.Repeticiones = RepeticionesStack.Children
            .OfType<Entry>()
            .Select(entry => entry.Text)
            .ToArray();

        var json = JsonSerializer.Serialize(rutina);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await new HttpClient().PostAsync("http://nekjoesg-001-site5.anytempurl.com/api/Rutina", content);

        if (response.IsSuccessStatusCode)
        {
            ClearRutinaForm();
            await DisplayAlert("Éxito", "Rutina guardada correctamente.", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Ocurrió un error al guardar la rutina.", "OK");
        }
    }

    private void ClearRutinaForm()
    {
        RutinaNombreEntry.Text = string.Empty;
        RutinaTituloEntry.Text = string.Empty;
        RutinaSubtituloEntry.Text = string.Empty;
        RutinaDiaPicker.SelectedItem = null;
        RutinaUserIdPicker.SelectedItem = null;

        PosicionInicialCountEntry.Text = string.Empty;
        MovimientosCountEntry.Text = string.Empty;
        RespiracionesCountEntry.Text = string.Empty;
        RepeticionesCountEntry.Text = string.Empty;

        PosicionInicialStack.Children.Clear();
        MovimientosStack.Children.Clear();
        RespiracionesStack.Children.Clear();
        RepeticionesStack.Children.Clear();

        _image1Rutina = null;
        _image2Rutina = null;
        RutinaSelectedImage1.Source = null;
        RutinaSelectedImage2.Source = null;
    }

    private async void OnSelectVideoImage2Clicked(object sender, EventArgs e)
    {
        var selectedImageFile = await FilePicker.Default.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Seleccione una imagen"
        });

        if (selectedImageFile != null)
        {
            var stream = await selectedImageFile.OpenReadAsync();
            _image2Rutina = selectedImageFile;
            RutinaSelectedImage2.Source = ImageSource.FromStream(() => stream);
        }
    }

    private async void OnSelectVideoImage1Clicked(object sender, EventArgs e)
    {
        var selectedImageFile = await FilePicker.Default.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Seleccione una imagen"
        });

        if (selectedImageFile != null)
        {
            var stream = await selectedImageFile.OpenReadAsync();

            _image1Rutina = selectedImageFile;
            RutinaSelectedImage1.Source = ImageSource.FromStream(() => stream);
        }
    }
}