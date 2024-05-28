using System.Collections.ObjectModel;
using AppSharedClasses.Models;

public class ComboBoxItem<T>
{
    public T ComboBoxOption { get; set; }
    public string ComboBoxHumanReadableOption { get; set; }
}

public class StarterPageModel
{
    public int experimentNumber;
    public int age;
    public string name;
    
    public UserType userType;
    public ObservableCollection<ComboBoxItem<UserType>> userTypeOptions;
    public int userTypeOptionIndex;

    public ObservableCollection<ComboBoxItem<Cases>> expetipemtOptions;
    public int expetipemtOptionIndex;

    public bool IsValid => !string.IsNullOrEmpty(name);

    public StarterPageModel() 
    {
        expetipemtOptions = new ObservableCollection<ComboBoxItem<Cases>>();
        expetipemtOptions.Add(
            new ComboBoxItem<Cases>
            {
                ComboBoxOption = Cases.Shape,
                ComboBoxHumanReadableOption = "Фигуры"
            });

        expetipemtOptions.Add(
            new ComboBoxItem<Cases>
            {
                ComboBoxOption = Cases.Images,
                ComboBoxHumanReadableOption = "Изображения и звук"
            });

        userTypeOptions = new ObservableCollection<ComboBoxItem<UserType>>();
        userTypeOptions.Add(
            new ComboBoxItem<UserType>
            {
                ComboBoxOption = UserType.Elementary,
                ComboBoxHumanReadableOption = "Начальный"
            });

        userTypeOptions.Add(
             new ComboBoxItem<UserType>
             {
                 ComboBoxOption = UserType.Experienced,
                 ComboBoxHumanReadableOption = "Опытный"
             });

        userTypeOptions.Add(
            new ComboBoxItem<UserType>
            {
                ComboBoxOption = UserType.Advanced,
                ComboBoxHumanReadableOption = "Продвинутый"
            });

        expetipemtOptionIndex = 0;
        age = 18;
    }
}

