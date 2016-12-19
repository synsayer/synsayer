#if !UNITY_WEBPLAYER

using System.Net;
using System.IO;
using System;
using System.Text;

namespace FrameWork.Util.Uploader
{

    public class FTPClient
    {

        // The hostname or IP address of the FTP server
        private string _remoteHost;

        // The remote username
        private string _remoteUser;

        // Desire to upload path
        private string _desirePath;


        // Password for the remote user
        private string _remotePass;


        public FTPClient(string remoteHost, string remoteUser, string remotePassword, string desirePath = "")
        {
            _remoteHost = remoteHost;
            _desirePath = desirePath;
            _remoteUser = remoteUser;
            _remotePass = remotePassword;
        }





        public string DirectoryListing()
        {
            #if UNITY_SAMSUNGTV
                return "";
            #else
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_remoteHost + _desirePath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(_remoteUser, _remotePass);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                string result = string.Empty;

                while (!reader.EndOfStream)
                {
                    result += reader.ReadLine() + Environment.NewLine;
                }

                reader.Close();
                response.Close();
                return result;
            #endif
        }




        public string DirectoryListing(string folder)
        {
            #if UNITY_SAMSUNGTV
                return "";
            #else

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_remoteHost + folder);

                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(_remoteUser, _remotePass);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                string result = string.Empty;

                while (!reader.EndOfStream)
                {
                    result += reader.ReadLine() + Environment.NewLine;
                }

                reader.Close();
                response.Close();
                return result;
            #endif
        }


        public void Download(string file, string destination)
        {
            #if UNITY_SAMSUNGTV
            #else

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_remoteHost + file);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(_remoteUser, _remotePass);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                StreamWriter writer = new StreamWriter(destination);
                writer.Write(reader.ReadToEnd());

                writer.Close();
                reader.Close();
                response.Close();
            #endif
        }



        public void UploadFile(string FullPathFilename)
        {
            #if UNITY_SAMSUNGTV
            #else
                string filename = Path.GetFileName(FullPathFilename);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_remoteHost + _desirePath + filename);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_remoteUser, _remotePass);

                StreamReader sourceStream = new StreamReader(FullPathFilename);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());

                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);

                requestStream.Close();
                sourceStream.Close();
            #endif
        }



    }
}

#endif