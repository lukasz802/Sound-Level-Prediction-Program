using Compute_Engine.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using static SoundLevelCalculator.Controls.NumericUpDown;
using static SoundLevelCalculator.SharedFunctions;

namespace SoundLevelCalculator.ViewModels
{
    public class DuctElementViewModel : INotifyPropertyChanged
    {
        #region Fields and Constans

        private Units airVolumeUnit;
        private Units lengthUnit;
        private Duct selectedDuctElement = new Duct();
        private ObservableCollection<KeyValuePair<int, double>> noiseLevelList;
        private ObservableCollection<KeyValuePair<int, double>> attenuationLevelList;
        private ObservableCollection<Duct> diameterList;
        private ObservableCollection<Duct> rectangularList;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properies

        public ICommand GetElementDiameter { get; private set; }

        public ICommand GetElementRectangular { get; private set; }

        public ObservableCollection<KeyValuePair<int, double>> NoiseLevelList
        {
            get
            {
                if (noiseLevelList == null)
                {
                    noiseLevelList = new ObservableCollection<KeyValuePair<int, double>>(new KeyValuePair<int, double>[8]);
                }

                return noiseLevelList;
            }
            private set
            {
                noiseLevelList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<KeyValuePair<int, double>> AttenuationLevelList
        {
            get
            {
                if (attenuationLevelList == null)
                {
                    attenuationLevelList = new ObservableCollection<KeyValuePair<int, double>>(new KeyValuePair<int, double>[8]);
                }

                return attenuationLevelList;
            }
            private set
            {
                attenuationLevelList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Duct> DiametersList
        {
            get
            {
                if (diameterList == null)
                {
                    PrepareDiametersList();
                }

                return diameterList;
            }
            private set
            {
                diameterList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Duct> RectangularList
        {
            get
            {
                if (rectangularList == null)
                {
                    PrepareRectangularList();
                }

                return rectangularList;
            }
            private set
            {
                rectangularList = value;
                OnPropertyChanged();
            }
        }

        public bool IsIsolated
        {
            get
            {
                return selectedDuctElement.LinerCheck;
            }
            set
            {
                selectedDuctElement.LinerCheck = value;
                UpdateAcousticLevelData();
                OnPropertyChanged();
            }
        }

        public int IsolationThickness
        {
            get
            {
                return selectedDuctElement.LinerDepth;
            }
            set
            {
                selectedDuctElement.LinerDepth = value;
                UpdateAcousticLevelData();
                OnPropertyChanged();
            }
        }

        public double ElementLength
        {
            get
            {
                return Convert.ToDouble(ConvertUnits(selectedDuctElement.Lenght, Units.Meters, LengthUnit));
            }
            set
            {
                selectedDuctElement.Lenght = Convert.ToDouble(ConvertUnits(value, LengthUnit, Units.Meters));
                UpdateAcousticLevelData();
                OnPropertyChanged();
            }
        }

        public Compute_Engine.Enums.DuctType DuctType
        {
            get
            {
                return selectedDuctElement.DuctType;
            }
            set
            {
                selectedDuctElement.DuctType = value;
                UpdateAcousticLevelData();
                OnPropertyChanged();
            }
        }

        public Units LengthUnit
        {
            get
            {
                return lengthUnit;
            }
            set
            {
                lengthUnit = value;
                OnPropertyChanged();
            }
        }

        public Units AirVolumeUnit
        {
            get
            {
                return airVolumeUnit;
            }
            set
            {
                airVolumeUnit = value;
                OnPropertyChanged();
            }
        }

        public double AirVolume
        {
            get
            {
                return Convert.ToDouble(ConvertUnits(selectedDuctElement.AirFlow, Units.CubicMetersPerHour, AirVolumeUnit));
            }
            set
            {
                selectedDuctElement.AirFlow = Convert.ToInt32(ConvertUnits(value, AirVolumeUnit, Units.CubicMetersPerHour));
                UpdateDimiensionLists();
                UpdateAcousticLevelData();
                OnPropertyChanged();
            }
        }

        public int ElementDiameter
        {
            get
            {
                return selectedDuctElement.Diameter;
            }
            set
            {
                selectedDuctElement.Diameter = value;
                OnPropertyChanged();
            }
        }

        public int ElementWidth
        {
            get
            {
                return selectedDuctElement.Width;
            }
            set
            {
                selectedDuctElement.Width = value;
                OnPropertyChanged();
            }
        }

        public int ElementHeight
        {
            get
            {
                return selectedDuctElement.Height;
            }
            set
            {
                selectedDuctElement.Height = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public DuctElementViewModel()
        {
            airVolumeUnit = Units.CubicMetersPerHour;
            lengthUnit = Units.Milimeters;
            IsolationThickness = 25;
            AirVolume = 200;
            DuctType = Compute_Engine.Enums.DuctType.Round;
            ElementLength = 1000;
            GetElementDiameter = new RelayCommand(GetDiamter);
            GetElementRectangular = new RelayCommand(GetRectangular);
        }

        #endregion

        #region Methods

        private void PrepareDiametersList()
        {
            Duct temp;
            DiametersList = new ObservableCollection<Duct>();

            foreach (string diameterNode in StaticResources.DiametersList)
            {
                temp = new Duct()
                {
                    DuctType = Compute_Engine.Enums.DuctType.Round,
                    Diameter = int.Parse(diameterNode),
                    AirFlow = Convert.ToInt32(ConvertUnits(AirVolume, AirVolumeUnit, Units.CubicMetersPerHour)),
                };

                DiametersList.Add(temp);
            }
        }

        private void PrepareRectangularList()
        {
            Duct temp;
            RectangularList = new ObservableCollection<Duct>();

            foreach (KeyValuePair<string, List<string>> kvp in StaticResources.RectangularList)
            {
                foreach (string element in kvp.Value)
                {
                    temp = new Duct()
                    {
                        DuctType = Compute_Engine.Enums.DuctType.Rectangular,
                        Width = int.Parse(kvp.Key),
                        Height = int.Parse(element),
                        AirFlow = Convert.ToInt32(ConvertUnits(AirVolume, AirVolumeUnit, Units.CubicMetersPerHour)),
                    };

                    RectangularList.Add(temp);
                }
            }
        }

        private void UpdateAcousticLevelData()
        {
            var noise = selectedDuctElement.Noise();
            var attenuation = selectedDuctElement.Attenuation();

            for (int i = 0; i < noise.Count(); i++)
            {
                NoiseLevelList[i] = new KeyValuePair<int, double>(i, Math.Round(noise[i], 1, MidpointRounding.AwayFromZero));
                AttenuationLevelList[i] = new KeyValuePair<int, double>(i, Math.Round(attenuation[i], 1, MidpointRounding.AwayFromZero));
            }
        }

        private void UpdateRectangularList()
        {
            foreach (KeyValuePair<string, List<string>> kvp in StaticResources.RectangularList)
            {
                foreach (Duct element in RectangularList)
                {
                    element.AirFlow = Convert.ToInt32(ConvertUnits(AirVolume, AirVolumeUnit, Units.CubicMetersPerHour));
                }
            }
        }

        private void UpdateDiameterList()
        {
            foreach (Duct element in DiametersList)
            {
                element.AirFlow = Convert.ToInt32(ConvertUnits(AirVolume, AirVolumeUnit, Units.CubicMetersPerHour));
            }
        }

        private void GetDiamter(object obj)
        {
            ElementDiameter = ((Duct)obj).Diameter;
            UpdateAcousticLevelData();
        }

        private void GetRectangular(object obj)
        {
            ElementWidth = ((Duct)obj).Width;
            ElementHeight = ((Duct)obj).Height;
            UpdateAcousticLevelData();
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateDimiensionLists()
        {
            UpdateRectangularList();
            UpdateDiameterList();
        }

        #endregion
    }
}
