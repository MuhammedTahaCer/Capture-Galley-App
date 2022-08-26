using Syncfusion.SfCarousel.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        public String PhotoPath { get; set; }
        public int i = 0;

        ObservableCollection<Images> obImages;

        public MainPage()
        {
            InitializeComponent();
            AddImages();
            var labeltext = "Eklenen Resimler!";

            /*CarouselPage carouselPage = new CarouselPage();

            List<ClassImages> images = new List<ClassImages>()
            {
                new ClassImages(){Title="1. Resim", Image=imj.Source },
                new ClassImages(){Title="2. Resim", Image=imj2.Source },
                new ClassImages(){Title="3. Resim", Image=imj3.Source }
            };
            carouselPage.ItemsSource = images;*/ // --> Bu arkadaş, ClassImages parametrelerini ref alıp liste halinde kaydedip / slide oluştturuyor. Xaml tarafında görüntüleyemedim.
        }

        public void AddImages()
        {
            try
            {
                obImages = new ObservableCollection<Images>();

                obImages.Add(new Images
                {
                    iImage = imj.Source,
                    iName = "1. Resim",
                });

                obImages.Add(new Images
                {
                    iImage = imj2.Source,
                    iName = "2. Resim",
                });

                obImages.Add(new Images
                {
                    iImage = imj3.Source,
                    iName = "3. Resim",
                });

            } catch { return; }
         }
      
        async Task TaskPhotAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"Fotoğraf Çekildi: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine(fnsEx.Message);
            }
            catch (PermissionException peX)
            {
                Console.WriteLine(peX.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }

            var newfile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newfile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newfile;

        }

        private void btnOpenCamera_Clicked(object sender, EventArgs e)
        {
            
            if (i<3)
            {
                i++;
                Device.BeginInvokeOnMainThread(async () =>
            {
                await TaskPhotAsync();
                if (i==1)
                {
                    imj.Source = ImageSource.FromFile(PhotoPath);
                    obImages.Add(new Images
                    {
                        iImage = imj.Source,
                        iName = "1. Resim",
                    });
                    captured.Source = imj.Source;

                }
                else if (i==2)
                {
                    frame2.SetValue(IsVisibleProperty, true);
                    imj2.Source = ImageSource.FromFile(PhotoPath);
                    obImages.Add(new Images
                    {
                        iImage = imj2.Source,
                        iName = "2. Resim",
                    });
                    captured.Source = imj2.Source;
                }
                else
                {
                   frame3.SetValue(IsVisibleProperty, true);
                    imj3.Source = ImageSource.FromFile(PhotoPath);
                    obImages.Add(new Images
                    {
                        iImage = imj3.Source,
                        iName = "3. Resim",
                    });
                }
            });

            } else { DisplayAlert("Uyarı", "Yeterili sayıda fotoğraf çekildi", "Ok"); }
        }

        /*  private void btnReOpenCamera_Clicked(object sender, EventArgs e)
          {
              imj.Source = null;
              btnOpenCamera_Clicked(sender, EventArgs.Empty);
          } */



        private void edit_Tapped(object sender, EventArgs e)
        {
            i = 0;
            imj.Source = null;
            btnOpenCamera_Clicked(sender, EventArgs.Empty);
        }
        private void edit1_Tapped(object sender, EventArgs e)
        {
            i = 1;
            imj2.Source = null;
            btnOpenCamera_Clicked(sender, EventArgs.Empty);
        }
        private void edit2_Tapped(object sender, EventArgs e)
        {
            i = 2;
            imj3.Source = null;
            btnOpenCamera_Clicked(sender, EventArgs.Empty);
        }

        private void zoom_Tapped(object sender, EventArgs e)
        {
            imj.Source= ImageSource.FromFile(PhotoPath);
            var imageSource = imj.Source;
            Navigation.PushModalAsync(new IpopUp(imageSource));
        }
        private void zoom1_Tapped(object sender, EventArgs e)
        {
            imj2.Source = ImageSource.FromFile(PhotoPath);
            var imageSource = imj2.Source;
            Navigation.PushModalAsync(new IpopUp(imageSource));
        }
        private void zoom2_Tapped(object sender, EventArgs e)
        {
            imj3.Source = ImageSource.FromFile(PhotoPath);
            var imageSource = imj3.Source;
            Navigation.PushModalAsync(new IpopUp(imageSource));
        }
        private async void lstImages_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var selectedImages = (Images)e.Item;

                await DisplayAlert("Seçiminiz:", selectedImages.iName, "Peki", "Geri Dön");

            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata Oluştu", ex.Message.ToString(), "Peki");
            }
        }
    }
}
    

