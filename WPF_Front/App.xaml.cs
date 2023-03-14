using System.IO;
using System.Reflection;

namespace WPF_Front
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string GetVersion()
        {
            string version;
            try
            {
                var fromLocal = File.ReadAllLines(Environment.CurrentDirectory + @"\WPF_Front.exe.manifest")[2];

                var versionLocal = fromLocal.Substring(
                    fromLocal.IndexOf("version="),
                    fromLocal.IndexOf("publicKeyToken=") - fromLocal.IndexOf("version="));
                var tmp = versionLocal.Substring(9);
                version = tmp.Remove(tmp.Length - 2);
            }
            catch
            {
                version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            }
            return version;
        }
    }
}
// DUCK //
/*
                                                              @@@@@@@@@@@
                                                         @@@@@@@@@@@@@@@@@@@@@@
                                                      @@@@@@@                 @@@@
                                                    @@@@@                        @@@@@
 @@@@@@@@@@@@@@@@                                  @@@@                             @@@@
 @@@           @@@@@@@                            @@@                                 @@@
  @@@               @@@@@@                       @@@                                   @@@@ 
    @@@@                 @@@@@                   @@@      @@@                           @@@@
       @@@@                 @@@@                 @@@                   @@@                @@@
          @@@@                 @@@@@             @@@@                                      @@
            &@@@                   @@@@@         @@@@  @@@@@@@@@@@&                        @@
  @@@@@@@@@@@@@@@@@                    @@@@@@@@@@@@@@@@............@@@@@@@                 @@@
@@@@@        @@@@@@@                       @@@@@@@......................@@@                @@@
@@@@@                                   @@@@@@@.........................@@@                @@@
 #@@@@                                @@@@@............................@@@@                @@@
    @@@@                            @@@@@.......@@@@......@@@@@.......@@@@                 @@@
      @@@@                         @@@@@...........@@...@@@.........@@@@#                  @@@
        @@@@                       @@@@...........................@@@@@                    /@@
           @@@@@                  @@@@..........................@@@@@                      @@@
              @@@@                @@@@.......................@@@@@                         .@@
             @@@@@@@@              @@@@@@@@@@%........%@@@@@@@@                             /@@
           @@@@@@@@@@@@               @@@@@@@@@@@@@@@@@@@@                                     @
           @@@                            @@@@                           
            @@@@                          @@@/
             @@@@@                        @@@
                @@@@                      @@@
                  @@@@                    @@@
                   @@@@@                  @@@@
                     @@@@@                /@@@                /--KWAK--\
                       @@@@@@              @@@
                          @@@@@@@@         @@@
                            &@@@@@@@@@@@@( @@@@
                                @@@@@@@@@@@@@@@@@
                                       @@@@@@@@@@@@                                      @@@@@
                                              @@@@@@@@.                                  @@@@@@
                                                     @@@@@@                         @@@@@@@
                                                         @@@@@@@@@@@@@@@@@@@@@@@@@@@@
*/