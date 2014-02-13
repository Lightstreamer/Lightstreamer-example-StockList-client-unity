import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.charset.Charset;
import java.security.InvalidParameterException;


public class Sockpol extends ServerSocket {

    class RequestWorker extends Thread {

    	private Socket socket;
    	private String policy;

    	RequestWorker(Socket socket, String policy) {
    		this.socket = socket;
    		this.policy = policy;
    	}

        private int read(InputStream is) throws IOException {
            /* read client HTTP request data,
             * using BufferedReader is not a problem here, the whole request
             * won't be considered at all. It's just for logging purposes. */
            InputStreamReader isr = new InputStreamReader(is);
            int len = 0;
            while (isr.ready()) {
                char[] buf = new char[8192];
                len += isr.read(buf, 0, 8192);
            }
            return len;
        }

    	public void run() {
    		try {
    			socket.setKeepAlive(true);
    			InetAddress client = socket.getInetAddress();
                System.out.println(client.toString() + " handling new connection");
            	File polf = new File(policy);
            	FileInputStream fis = new FileInputStream(polf.getAbsolutePath()); 
            	InputStreamReader in = new InputStreamReader(fis,
        				Charset.defaultCharset().displayName());
            	
            	this.read(socket.getInputStream());
            	OutputStream os = socket.getOutputStream();
            	char[] buf = new char[4096];
            	while (true) {
            		int got = in.read(buf);
            		if (got < 1)
            			/* EOF */
            			break;
        			os.write(new String(buf).getBytes(Charset.defaultCharset()), 0, got);
            	}
            	os.flush();
            	os.close();
    		} catch (IOException ioe) {
    			/* do nothing */
    			ioe.printStackTrace();
    		} finally {
    			try {
					socket.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
    		}
    	}
    }
	
	private String policy;
	private static int listenPort = 843;
	private static String listenAddress = "0.0.0.0";
	
	public Sockpol(String policy) throws IOException {
		super(listenPort, 1024, InetAddress.getByName(listenAddress));
		this.policy = policy;
	}

	public void start() throws IOException {
        while (true) {
            /* select() */
            Socket socket = this.accept();
            RequestWorker worker = new RequestWorker(socket, policy);
            worker.start();
        }
	}

	public static void main(String args[]) throws IOException {

		if (args.length < 1) {
			System.err.println("Need a path to a policy file");
			throw new InvalidParameterException();
		}
		String policy = args[0];
		// verify path
		File pol = new File(policy);
		if (!(pol.isFile() && pol.canRead())) {
			System.err.println("Policy file provided does not exist (or invalid permissions)");
			throw new InvalidParameterException();
		}

		Sockpol sockpol = new Sockpol(policy);
		sockpol.start();

	}
	
}
