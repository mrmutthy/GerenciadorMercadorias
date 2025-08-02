using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GerenciadorMercadorias.Model;
using GerenciadorMercadorias.DAL;

namespace GerenciadorMercadorias.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private MercadoriaDAL _mercadoriaDAL;
        private const string ConnectionString = "Data Source=mercadorias.db";

        public ObservableCollection<Mercadoria> Mercadorias { get; set; }

        private string _nomeMercadoria = string.Empty;
        public string NomeMercadoria
        {
            get => _nomeMercadoria;
            set { _nomeMercadoria = value; OnPropertyChanged(); }
        }

        private string? _descricaoMercadoria;
        public string? DescricaoMercadoria
        {
            get => _descricaoMercadoria;
            set { _descricaoMercadoria = value; OnPropertyChanged(); }
        }

        private decimal _precoMercadoria;
        public decimal PrecoMercadoria
        {
            get => _precoMercadoria;
            set { _precoMercadoria = value; OnPropertyChanged(); }
        }

        private int _estoqueMercadoria;
        public int EstoqueMercadoria
        {
            get => _estoqueMercadoria;
            set { _estoqueMercadoria = value; OnPropertyChanged(); }
        }

        private bool _listaVisivel;
        public bool ListaVisivel
        {
            get => _listaVisivel;
            set { _listaVisivel = value; OnPropertyChanged(); }
        }

        private bool _botoesEdicaoVisiveis;
        public bool BotoesEdicaoVisiveis
        {
            get => _botoesEdicaoVisiveis;
            set { _botoesEdicaoVisiveis = value; OnPropertyChanged(); }
        }

        private Mercadoria? _selectedMercadoria;
        public Mercadoria? SelectedMercadoria
        {
            get => _selectedMercadoria;
            set
            {
                _selectedMercadoria = value;
                OnPropertyChanged();
                if (value != null)
                {
                    NomeMercadoria = value.Nome;
                    DescricaoMercadoria = value.Descricao;
                    PrecoMercadoria = value.Preco;
                    EstoqueMercadoria = value.Estoque;
                    BotoesEdicaoVisiveis = true;
                }
                else
                {
                    BotoesEdicaoVisiveis = false;
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AdicionarCommand { get; }
        public ICommand AtualizarCommand { get; }
        public ICommand ExcluirCommand { get; }
        public ICommand ListarCommand { get; }

        public MainWindowViewModel()
        {
            _mercadoriaDAL = new MercadoriaDAL(ConnectionString);
            Mercadorias = new ObservableCollection<Mercadoria>();

            AdicionarCommand = new RelayCommand(AdicionarMercadoria);
            AtualizarCommand = new RelayCommand(AtualizarMercadoria, _ => SelectedMercadoria != null);
            ExcluirCommand = new RelayCommand(ExcluirMercadoria, _ => SelectedMercadoria != null);
            ListarCommand = new RelayCommand(ListarMercadorias);

            ListaVisivel = false;
            BotoesEdicaoVisiveis = false;
        }

        private void ListarMercadorias(object? parameter)
        {
            CarregarMercadorias();
            ListaVisivel = true;
            BotoesEdicaoVisiveis = false;
            SelectedMercadoria = null;
        }

        private void CarregarMercadorias()
        {
            Mercadorias.Clear();
            foreach (var mercadoria in _mercadoriaDAL.ListarMercadorias())
            {
                Mercadorias.Add(mercadoria);
            }
        }

        private void AdicionarMercadoria(object? parameter)
        {
            if (string.IsNullOrWhiteSpace(NomeMercadoria))
            {
                System.Windows.MessageBox.Show("O campo Nome é obrigatório.");
                return;
            }
            if (PrecoMercadoria <= 0)
            {
                System.Windows.MessageBox.Show("O campo Preço deve ser maior que zero.");
                return;
            }
            if (EstoqueMercadoria < 0)
            {
                System.Windows.MessageBox.Show("O campo Estoque não pode ser negativo.");
                return;
            }
            try
            {
                var novaMercadoria = new Mercadoria
                {
                    Nome = NomeMercadoria,
                    Descricao = DescricaoMercadoria,
                    Preco = PrecoMercadoria,
                    Estoque = EstoqueMercadoria
                };
                _mercadoriaDAL.AdicionarMercadoria(novaMercadoria);
                if (ListaVisivel)
                    CarregarMercadorias();
                LimparCampos();
                System.Windows.MessageBox.Show("Adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Erro ao adicionar: " + ex.Message);
            }
        }


        private void AtualizarMercadoria(object? parameter)
        {
            if (SelectedMercadoria != null)
            {
                try
                {
                    SelectedMercadoria.Nome = NomeMercadoria;
                    SelectedMercadoria.Descricao = DescricaoMercadoria;
                    SelectedMercadoria.Preco = PrecoMercadoria;
                    SelectedMercadoria.Estoque = EstoqueMercadoria;

                    _mercadoriaDAL.AtualizarMercadoria(SelectedMercadoria);
                    CarregarMercadorias();
                    LimparCampos();
                    System.Windows.MessageBox.Show("Atualizado com sucesso!");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao atualizar: " + ex.Message);
                }
            }
        }


        private void ExcluirMercadoria(object? parameter)
        {
            if (SelectedMercadoria != null)
            {
                try
                {
                    _mercadoriaDAL.ExcluirMercadoria(SelectedMercadoria.Id);
                    CarregarMercadorias();
                    LimparCampos();
                    System.Windows.MessageBox.Show("Excluído com sucesso!");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao excluir: " + ex.Message);
                }
            }
        }

        private void LimparCampos()
        {
            NomeMercadoria = string.Empty;
            DescricaoMercadoria = null;
            PrecoMercadoria = 0;
            EstoqueMercadoria = 0;
            SelectedMercadoria = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);
    }
}