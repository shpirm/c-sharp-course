using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Exceptions;
using WpfApp.Model;
using WpfApp.ViewModel;

public class UserViewModel : MainViewModel
{
    private readonly MainViewModel _mainViewModel;
    private readonly Person _selectedUser;
    private readonly Person _editableUser;
    public bool EditSuccessful { get; private set; }


    public enum Mode
    {
        Add,
        Edit
    }

    public Mode CurrentMode { get; private set; }
    public Person EditableUser => _editableUser;

    public ICommand SaveEditCommand { get; }

    public UserViewModel(MainViewModel mainViewModel, Person selectedUser = null)
    {
        _mainViewModel = mainViewModel;
        _selectedUser = selectedUser;
        CurrentMode = selectedUser == null ? Mode.Add : Mode.Edit;
        _editableUser = selectedUser?.Clone() ?? new Person();
        SaveEditCommand = new RelayCommand(async (parameter) => await SaveEditAsync(parameter));
    }

    private async Task SaveEditAsync(object parameter)
    {
        try
        {
            _editableUser.ValidatePerson();
            if (CurrentMode == Mode.Add)
            {
                await _mainViewModel.AddUserAsync(_editableUser);
                EditSuccessful = true;
                MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (CurrentMode == Mode.Edit)
            {
                await _mainViewModel.UpdateUserAsync(_selectedUser, _editableUser);
                EditSuccessful = true;
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
