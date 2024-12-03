using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TestSistemi;
using TestSistemi.Data;
using TestSistemi.Models;

public class MainViewModel : INotifyPropertyChanged
{
    // Evento per notificare i cambiamenti delle proprietà
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Repository per interagire con il database
    private readonly Repository _repository;

    // Lista osservabile per i prodotti
    public ObservableCollection<Product> Products { get; set; }

    // Prodotto selezionato nel DataGrid
    private Product _selectedProduct;
    public Product SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged(nameof(SelectedProduct));

            // Aggiorna lo stato dei comandi
            ((RelayCommand)UpdateProductCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteProductCommand).RaiseCanExecuteChanged();
        }
    }

    // Prodotto attualmente in fase di modifica o aggiunta
    private Product _currentProduct;
    public Product CurrentProduct
    {
        get => _currentProduct;
        set
        {
            _currentProduct = value;
            OnPropertyChanged(nameof(CurrentProduct));
        }
    }

    // Comandi
    public ICommand AddProductCommand { get; }
    public ICommand UpdateProductCommand { get; }
    public ICommand DeleteProductCommand { get; }

    // Costruttore
    public MainViewModel()
    {
        _repository = new Repository();
        Products = new ObservableCollection<Product>(_repository.GetAllProducts());

        // Inizializzazione del prodotto per l'inserimento
        CurrentProduct = new Product();

        // Inizializzazione dei comandi
        AddProductCommand = new RelayCommand(AddProduct);
        UpdateProductCommand = new RelayCommand(UpdateProduct, CanUpdateOrDelete);
        DeleteProductCommand = new RelayCommand(DeleteProduct, CanUpdateOrDelete);
    }

    // Metodo per aggiungere un prodotto
    public void AddProduct()
    {
        // Verifica che tutti i campi siano riempiti
        if (string.IsNullOrWhiteSpace(CurrentProduct.ProductName) || CurrentProduct.Quantity <= 0 || CurrentProduct.Price <= 0)
        {
            MessageBox.Show("Per favore, riempi tutti i campi correttamente!", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Salva nel database e aggiorna la lista
        _repository.AddProduct(CurrentProduct);
        Products.Add(CurrentProduct);

        // Reset del prodotto corrente
        CurrentProduct = new Product();

        MessageBox.Show("Prodotto aggiunto con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    // Metodo per aggiornare un prodotto
    public void UpdateProduct()
    {
        if (SelectedProduct != null)
        {
            try
            {
                _repository.UpdateProduct(SelectedProduct);
                MessageBox.Show("Prodotto aggiornato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'aggiornamento del prodotto: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Seleziona un prodotto da modificare.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Metodo per eliminare un prodotto
    public void DeleteProduct()
    {
        if (SelectedProduct != null)
        {
            var result = MessageBox.Show(
                $"Sei sicuro di voler eliminare il prodotto '{SelectedProduct.ProductName}'?",
                "Conferma Eliminazione",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.DeleteProduct(SelectedProduct.ProductID);
                    Products.Remove(SelectedProduct);
                    SelectedProduct = null;

                    MessageBox.Show("Prodotto eliminato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore durante l'eliminazione del prodotto: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Seleziona un prodotto da eliminare.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Metodo per verificare se i comandi possono essere eseguiti
    public bool CanUpdateOrDelete()
    {
        return SelectedProduct != null;
    }
}
