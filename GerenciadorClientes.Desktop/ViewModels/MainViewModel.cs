using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GerenciadorClientes.Core.Data;
using GerenciadorClientes.Core.Models;

namespace GerenciadorClientes.Desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    // Mesmo DAO usado pelo ConsoleApp, vindo da camada Core.
    private readonly ClienteDAO _clienteDao = new();

    // ObservableCollection avisa a tela sozinha a cada item adicionado ou removido.
    public ObservableCollection<Cliente> Clientes { get; } = new();

    // [ObservableProperty] gera a notificacao de mudanca para a tela.
    [ObservableProperty]
    public partial string Status { get; set; } = string.Empty;

    public MainViewModel()
    {
        // O preview da IDE cria este ViewModel para desenhar a tela. Sem esta
        // guarda, ele tentaria conectar no Oracle a cada edicao do XAML.
        if (Design.IsDesignMode)
        {
            CarregarExemploParaPreview();
            return;
        }

        CarregarClientes();
    }

    // [RelayCommand] gera a propriedade CarregarClientesCommand, usada no botao.
    [RelayCommand]
    private void CarregarClientes()
    {
        try
        {
            Clientes.Clear();

            foreach (Cliente cliente in _clienteDao.ListarTodos())
            {
                Clientes.Add(cliente);
            }

            Status = Clientes.Count == 0
                ? "Nenhum cliente cadastrado."
                : $"{Clientes.Count} cliente(s) carregado(s).";
        }
        catch (Exception ex)
        {
            // Sem isso, falha de conexao derrubaria a janela inteira.
            Status = $"Erro ao acessar o banco: {ex.Message}";
        }
    }

    // Dados ficticios para a tela nao aparecer vazia no preview da IDE.
    private void CarregarExemploParaPreview()
    {
        Clientes.Add(new Cliente("Maria Souza", "12345678901", "maria@email.com", "47999990000")
            { Id = 1, DataCadastro = DateTime.Today });
        Clientes.Add(new Cliente("Joao Lima", "98765432100", "joao@email.com", "47988887777")
            { Id = 2, DataCadastro = DateTime.Today });

        Status = "Pre-visualizacao com dados de exemplo.";
    }
}
