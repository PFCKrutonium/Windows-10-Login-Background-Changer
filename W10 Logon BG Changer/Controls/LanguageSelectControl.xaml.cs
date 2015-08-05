﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using W10_Logon_BG_Changer.Tools.Models;

namespace W10_Logon_BG_Changer.Controls
{
    /// <summary>
    /// Interaction logic for LanguageSelectControl.xaml
    /// </summary>
    public partial class LanguageSelectControl : UserControl
    {
        private readonly MainWindow _mainWindow;
        public readonly ObservableCollection<LangFormatTree> Names = new ObservableCollection<LangFormatTree>();

        public LanguageSelectControl(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();

            var langs = from pair in LanguageLibrary.Language.GetLangNames()
                        orderby pair.Key ascending
                        select pair;

            foreach (var lang in langs)
            {
                var enabled = ((string)LanguageLibrary.Language.Default.lang_name).Equals(lang.Key,
                    StringComparison.CurrentCultureIgnoreCase);
                Names.Add(new LangFormatTree(string.Format("{0} ({1})", lang.Key, lang.Value), lang.Value, !enabled));
            }

            SelectedLanguageView.ItemsSource = Names;
        }

        private void LanguageClicked_Event(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            var code = (string)btn.Tag;

            foreach (var name in Names)
            {
                Debug.WriteLineIf(code == name.LangCode, "code == name.LangCode");
                name.Enabled = !code.Equals(name.LangCode, StringComparison.CurrentCultureIgnoreCase);
            }

            MessageBox.Show("Language is set. Application must restart to apply language", "Language set");
            Settings.Default.Set("language", code);
            Settings.Default.Save();

            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}