using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Cryptlex
{
    public class LexFloatClient
    {
        private const string DLL_FILE_NAME = "LexFloatClient.dll";

        /*
            In order to use "Any CPU" configuration, rename 64 bit LexFloatClient.dll to LexFloatClient64.dll and add "LF_ANY_CPU"
	        conditional compilation symbol in your project properties.
        */
#if LF_ANY_CPU
        private const string DLL_FILE_NAME_X64 = "LexFloatClient64.dll";
#endif

        private string productId = null;
        private uint handle = 0;

        /*
            FUNCTION: SetProductId()

            PURPOSE: Sets the product id of your application.

            PARAMETERS:
            * productId - the unique product id of your application as mentioned
            on the product page in the dashboard.

            RETURN CODES: LF_OK, LF_E_PRODUCT_ID
        */

        public int SetProductId(string productId)
        {
            this.productId = productId;
#if LF_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetHandle_x64(productId, ref handle) : Native.GetHandle(productId, ref handle);
#else
            return Native.GetHandle(productId, ref handle);
#endif
        }

        /*
            FUNCTION: SetFloatServer()

            PURPOSE: Sets the network address of the LexFloatServer.

            PARAMETERS:
            * handle - handle for the product id
            * hostAddress - hostname or the IP address of the LexFloatServer
            * port - port of the LexFloatServer

            RETURN CODES: LF_OK, LF_E_HANDLE, LF_E_PRODUCT_ID, LF_FAIL
        */

        public int SetFloatServer(string hostAddress, ushort port)
        {
#if LF_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetFloatServer_x64(handle, hostAddress, port) : Native.SetFloatServer(handle, hostAddress, port);
#else
            return Native.SetFloatServer(handle, hostAddress, port);
#endif

        }

        /*
            FUNCTION: SetLicenseCallback()

            PURPOSE: Sets refresh license error callback function.

            Whenever the lease expires, a refresh lease request is sent to the
            server. If the lease fails to refresh, refresh license callback function
            gets invoked with the following status error codes: LF_E_LICENSE_EXPIRED,
            LF_E_LICENSE_EXPIRED_INET, LF_E_SERVER_TIME, LF_E_TIME.

            PARAMETERS:
            * handle - handle for the product id
            * callback - name of the callback function

            RETURN CODES: LF_OK, LF_E_HANDLE, LF_E_PRODUCT_ID
        */

        public int SetLicenseCallback(CallbackType callback)
        {
            var wrappedCallback = callback;
            var syncTarget = callback.Target as System.Windows.Forms.Control;
            if (syncTarget != null)
            {
                wrappedCallback = (v) => syncTarget.Invoke(callback, new object[] { v });
            }
#if LF_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetLicenseCallback_x64(handle, wrappedCallback) : Native.SetLicenseCallback(handle, wrappedCallback);
#else
            return Native.SetLicenseCallback(handle, wrappedCallback);
#endif

        }

        /*
            FUNCTION: RequestLicense()

            PURPOSE: Sends the request to lease the license from the LexFloatServer.

            PARAMETERS:
            * handle - handle for the product id

            RETURN CODES: LF_OK, LF_FAIL, LF_E_HANDLE, LF_E_PRODUCT_ID, LF_E_SERVER_ADDRESS,
            LF_E_CALLBACK, LF_E_LICENSE_EXISTS, LF_E_INET, LF_E_NO_FREE_LICENSE, LF_E_TIME,
            LF_E_PRODUCT_VERSION, LF_E_SERVER_TIME
        */

        public int RequestLicense()
        {
#if LF_ANY_CPU
            return IntPtr.Size == 8 ? Native.RequestLicense_x64(handle) : Native.RequestLicense(handle);
#else
            return Native.RequestLicense(handle);
#endif

        }

        /*
            FUNCTION: DropLicense()

            PURPOSE: Sends the request to drop the license from the LexFloatServer.

            Call this function before you exit your application to prevent zombie licenses.

            PARAMETERS:
            * handle - handle for the product id

            RETURN CODES: LF_OK, LF_FAIL, LF_E_HANDLE, LF_E_PRODUCT_ID, LF_E_SERVER_ADDRESS,
            LF_E_CALLBACK, LF_E_INET, LF_E_TIME, LF_E_SERVER_TIME
        */

        public int DropLicense()
        {
#if LF_ANY_CPU
            return IntPtr.Size == 8 ? Native.DropLicense_x64(handle) : Native.DropLicense(handle);
#else
            return Native.DropLicense(handle);
#endif

        }

        /*
            FUNCTION: HasLicense()

            PURPOSE: Checks whether any license has been leased or not. If yes,
            it retuns LF_OK.

            PARAMETERS:
            * handle - handle for the product id

            RETURN CODES: LF_OK, LF_FAIL, LF_E_HANDLE, LF_E_PRODUCT_ID, LF_E_SERVER_ADDRESS,
            LF_E_CALLBACK
        */

        public int HasLicense()
        {
#if LF_ANY_CPU
            return IntPtr.Size == 8 ? Native.HasLicense_x64(handle) : Native.HasLicense(handle);
#else
            return Native.HasLicense(handle);
#endif

        }

        /*
            FUNCTION: GetLicenseMetadata()

            PURPOSE: Get the value of the license metadata field associated with the float server key.

            PARAMETERS:
            * handle - handle for the product id
            * key - key of the metadata field whose value you want to get
            * value - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the value parameter

            RETURN CODES: LF_OK, LF_FAIL, LF_E_HANDLE, LF_E_PRODUCT_ID, LF_E_SERVER_ADDRESS,
            LF_E_CALLBACK, LF_E_BUFFER_SIZE, LF_E_METADATA_KEY_NOT_FOUND, LF_E_INET, LF_E_TIME,
            LF_E_SERVER_TIME
        */

        public int GetLicenseMetadata(string key, StringBuilder value, int length)
        {
#if LF_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetLicenseMetadata_x64(handle, key, value, length) : Native.GetLicenseMetadata(handle, key, value, length);
#else
            return Native.GetLicenseMetadata(handle, key, value, length);
#endif

        }

        /*
            FUNCTION: GlobalCleanUp()

            PURPOSE: Releases the resources acquired for sending network requests.

            Call this function before you exit your application.

            RETURN CODES: LF_OK

            NOTE: This function does not drop any leased license on the LexFloatServer.
        */

        public static int GlobalCleanUp()
        {
#if LF_ANY_CPU
            return IntPtr.Size == 8 ? Native.GlobalCleanUp_x64() : Native.GlobalCleanUp();
#else
            return Native.GlobalCleanUp();
#endif

        }

        public static class StatusCodes
        {
            /*
                CODE: LF_OK

                MESSAGE: Success code.
            */
            public const int LF_OK = 0;

            /*
                CODE: LF_FAIL

                MESSAGE: Failure code.
            */
            public const int LF_FAIL = 1;

            /*
                CODE: LF_E_PRODUCT_ID

                MESSAGE: The product id is incorrect.
            */
            public const int LF_E_PRODUCT_ID = 40;

            /*
                CODE: LF_E_CALLBACK

                MESSAGE: Invalid or missing callback function.
            */
            public const int LF_E_CALLBACK = 41;

            /*
                CODE: LF_E_HANDLE

                MESSAGE: Invalid handle.
            */
            public const int LF_E_HANDLE = 42;

            /*
                CODE: LF_E_SERVER_ADDRESS

                MESSAGE: Missing or invalid server address.
            */
            public const int LF_E_SERVER_ADDRESS = 43;

            /*
                CODE: LF_E_SERVER_TIME

                MESSAGE: System time on Server Machine has been tampered with. Ensure
                your date and time settings are correct on the server machine.
            */

            public const int LF_E_SERVER_TIME = 44;

            /*
                CODE: LF_E_TIME

                MESSAGE: The system time has been tampered with. Ensure your date
                and time settings are correct.
            */
            public const int LF_E_TIME = 45;

            /*
                CODE: LF_E_INET

                MESSAGE: Failed to connect to the server due to network error.
            */
            public const int LF_E_INET = 46;

            /*
                CODE: LF_E_NO_FREE_LICENSE

                MESSAGE: No free license is available
            */

            public const int LF_E_NO_FREE_LICENSE = 47;

            /*
                CODE: LF_E_LICENSE_EXISTS

                MESSAGE: License has already been leased.
            */

            public const int LF_E_LICENSE_EXISTS = 48;

            /*
                CODE: LF_E_LICENSE_EXPIRED

                MESSAGE: License lease has expired. This happens when the
                request to refresh the license fails due to license been taken
                up by some other client.
            */

            public const int LF_E_LICENSE_EXPIRED = 49;

            /*
                CODE: LF_E_LICENSE_EXPIRED_INET

                MESSAGE: License lease has expired due to network error. This
                happens when the request to refresh the license fails due to
                network error.
            */

            public const int LF_E_LICENSE_EXPIRED_INET = 50;

            /*
                CODE: LF_E_BUFFER_SIZE

                MESSAGE: The buffer size was smaller than required.
            */
            public const int LF_E_BUFFER_SIZE = 51;

            /*
                CODE: LF_E_METADATA_KEY_NOT_FOUND

                MESSAGE: The metadata key does not exist.
            */
            public const int LF_E_METADATA_KEY_NOT_FOUND = 52;

            /*
                CODE: LF_E_SERVER

                MESSAGE: Server error.
            */
            public const int LF_E_SERVER = 70;

            /*
                CODE: LF_E_CLIENT

                MESSAGE: Client error.
            */
            public const int LF_E_CLIENT = 71;
        };

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallbackType(uint status);

        /* To prevent garbage collection of delegate, need to keep a reference */

        static CallbackType leaseCallback;

        static class Native
        {
            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetHandle(string productId, ref uint handle);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetFloatServer(uint handle, string hostAddress, ushort port);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetLicenseCallback(uint handle, CallbackType callback);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int RequestLicense(uint handle);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int DropLicense(uint handle);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int HasLicense(uint handle);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetLicenseMetadata(uint handle, string key, StringBuilder value, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GlobalCleanUp();

#if LF_ANY_CPU
            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetHandle", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetHandle_x64(string productId, ref uint handle);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetFloatServer", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetFloatServer_x64(uint handle, string hostAddress, ushort port);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetLicenseCallback", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetLicenseCallback_x64(uint handle, CallbackType callback);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "RequestLicense", CallingConvention = CallingConvention.Cdecl)]
            public static extern int RequestLicense_x64(uint handle);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "DropLicense", CallingConvention = CallingConvention.Cdecl)]
            public static extern int DropLicense_x64(uint handle);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "HasLicense", CallingConvention = CallingConvention.Cdecl)]
            public static extern int HasLicense_x64(uint handle);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetLicenseMetadata", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetLicenseMetadata_x64(uint handle, string key, StringBuilder value, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GlobalCleanUp", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GlobalCleanUp_x64();

#endif
        }
    }
}