﻿using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Selenium_Interface
{
    public delegate void RequestDelegate(string data);
    public class HttpProcessor
    {
        public TcpClient socket;
        private Stream inputStream;
        public StreamWriter outputStream;
        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();
        private static int MAX_POST_SIZE = 1 * 1024 * 1024; // 1MB
        private RequestDelegate callback;
        private Form MainFrm;
        public HttpProcessor(TcpClient _socket, Form _MainFrm, RequestDelegate _callback)
        {
            this.socket = _socket;
            MainFrm = _MainFrm;
            callback = _callback;
        }
        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        public void process()
        {
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            string data = "";
            inputStream = new BufferedStream(socket.GetStream());
            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try
            {
                parseRequest();
                readHeaders();
                if (http_method.Equals("POST"))
                {
                    data = handlePOSTRequest();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream.Close(); outputStream.Close();
            inputStream = null; outputStream = null; // bs = null; 
            socket.Close();
            ////////////////////////////////////
            MainFrm.Invoke(callback, data);
        }
        public void parseRequest()
        {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];
            //Console.WriteLine("starting: " + request);
        }
        public void readHeaders()
        {
            //Console.WriteLine("readHeaders()");
            String line;
            while ((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    //Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                //Console.WriteLine("header: {0}:{1}",name,value);
                httpHeaders[name] = value;
            }
        }
        private const int BUF_SIZE = 4096;
        private string handlePOSTRequest()
        {
            StringBuilder data;
            int content_len = 0;
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                data = new StringBuilder(content_len);
                if (content_len > MAX_POST_SIZE)
                {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                          content_len));
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    for (int i = 0; i < content_len; i++)
                        data.Append(Convert.ToChar(buf[i]));
                    if (numread == 0)
                    {
                        if (to_read == 0)
                            break;
                        else
                            throw new Exception("client disconnected during post");
                    }
                    to_read -= numread;
                }
                writeSuccess();
                return data.ToString();
            }
            return "";
        }
        public void writeSuccess(string content_type = "text/html")
        {
            outputStream.WriteLine("HTTP/1.0 200 OK");
            outputStream.WriteLine("Content-Type: " + content_type);
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
        public void writeFailure()
        {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }
    public class IWebDriver
    {
        private int port;
        private TcpListener listener;
        private bool is_active = true;
        private RequestDelegate callback;
        private Form MainFrm;
        private void listen()
        {
            listener = new TcpListener(port);
            listener.Start();
            while (is_active)
            {
                try
                {
                    TcpClient socket = listener.AcceptTcpClient();
                    HttpProcessor processor = new HttpProcessor(socket, MainFrm, callback);
                    Thread thread = new Thread(new ThreadStart(processor.process));
                    thread.Start();
                    Thread.Sleep(1);
                }
                catch { }
            }
        }
        public IWebDriver(Form _MainFrm, RequestDelegate _callback, int _port)
        {
            port = _port;
            callback = _callback;
            MainFrm = _MainFrm;
        }
        Thread thread;
        public void Start()
        {
            is_active = true;
            thread = new Thread(new ThreadStart(listen));
            thread.Start();
        }
        public void Stop()
        {
            is_active = false;
            listener.Stop();
            thread.Abort();
        }
    }
}
