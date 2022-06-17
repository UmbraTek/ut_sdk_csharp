using System;
using utapi.basic;
using utapi.common;

namespace utapi.flxiv
{
    class FlxiVlApiBase : _ServoApiBase
    {
        protected void _init_(SocketFP _socket_fp, UtrcClient bus_client, UtrcType tx_data)
        {
            base._init_(_socket_fp, bus_client, tx_data);
        }

        // """Close socket"""
        public void close()
        {
            this._close();
        }

        // """Connect actuator ID
        // Args:
        //     id (int): The ID number of the actuator
        //     virtual_id (int, optional): Only used for debugging. Defaults to 0.
        // """
        public void connect_to_id(int id, int virtual_id = 0)
        {
            this._connect_to_id(id, virtual_id);
        }

        // ############################################################
        // #                       Basic Api
        // ############################################################
        // """Get the uuid
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     uuid (string): The unique code of umbratek products is also a certificate of repair and warranty
        //                    12-bit string
        // """
        public Tuple<int, String> get_uuid()
        {
            return this._get_uuid();
        }

        // """Get the software version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     version (string): Software version, 12-bit string
        // """
        public Tuple<int, String> get_sw_version()
        {
            return this._get_sw_version();
        }

        // """Get the hardware version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     version (string): Hardware version, 12-bit string
        // """
        public Tuple<int, String> get_hw_version()
        {
            return this._get_hw_version();
        }

        // """Set the id number of the device
        // Args:
        //     id (int): id number [1-125]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_com_id(int id)
        {
            return this._set_com_id(id);
        }

        // """Set communication baud rate, which can only be set to the following baud rates:
        // 9600, 14400, 19200, 38400, 56000,
        // 115200,128000,230400,256000,460800,500,000,512000,600000,750000,
        // 921600,1000000,1500000,2000000,2500000,3000000,3500000,4000000,4500000,
        // 5000000,5500000,6000000,8000000,11250000
        // Args:
        //     baud (int): communication baud rate
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_com_baud(int baud)
        {
            return this._set_com_baud(baud);
        }

        // """Reset fault
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int reset_err()
        {
            return this._reset_err();
        }

        // """Restart the device
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int restart_driver()
        {
            return this._restart_driver();
        }

        // """Restore the parameters to factory settings
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int erase_parm()
        {
            return this._erase_parm();
        }

        // """Save the current parameter settings
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int saved_parm()
        {
            return this._saved_parm();
        }

        // ############################################################
        // #                       Ectension Api
        // ############################################################
        // """Get the temperature limit threshold
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     min (int): Minimum temperature alarm threshold
        //     max (int): Maximum temperature alarm threshold
        // """
        public Tuple<int, int, int> get_temp_limit()
        {
            return this._get_temp_limit();
        }

        // """Set the temperature limit threshold,
        // the minimum alarm threshold range [-20, 90],
        // the maximum alarm threshold range [-20, 90], in degrees Celsius
        // Args:
        //     min (int): Minimum temperature alarm threshold
        //     max (int): Maximum temperature alarm threshold
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_temp_limit(int min, int max)
        {
            return this._set_temp_limit(min, max);
        }

        // """Get the voltage limit threshold
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     min (int): Minimum voltage alarm threshold
        //     max (int): Maximum voltage alarm threshold
        // """
        public Tuple<int, int, int> get_volt_limit()
        {
            return this._get_volt_limit();
        }

        // """Set the voltage limit threshold,
        // the minimum alarm threshold range [18, 55],
        // the maximum alarm threshold range [18, 55], unit volt
        // Args:
        //     min (int): Minimum voltage alarm threshold
        //     max (int): Maximum voltage alarm threshold
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_volt_limit(int min, int max)
        {
            return this._set_volt_limit(min, max);
        }

        // ############################################################
        // #                       Control Api
        // ############################################################
        // """Get the operating mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     mode (int): operating mode of the arm
        //         1: Position mode
        //         3: Current mode
        //         4: Mixed mode
        // """
        public Tuple<int, int> get_motion_mode()
        {
            return this._get_motion_mode();
        }

        // """Set the operating mode
        // When the motion mode is set, the device will deactivate the motion enable and need to re-enable the motion
        // Args:
        //     mode (int): operating mode of the arm
        //         1: Position mode
        //         3: Current mode
        //         4: Mixed mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_motion_mode(int mode)
        {
            return this._set_motion_mode(mode);
        }

        // """Get motion enable status
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     enable (bool): 0 Disable servo, 1 Enable servo
        // """
        public Tuple<int, int> get_motion_enable()
        {
            return this._get_motion_enable();
        }

        // """Set motion enable status
        // Args:
        //     enable (bool): 0 Disable servo, 1 Enable servo
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_motion_enable(int mode)
        {
            return this._set_motion_enable(mode);
        }

        // """Get drive temperature
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     temp (float): temperature [degrees Celsius]
        // """
        public Tuple<int, float> get_temp_driver()
        {
            return this._get_temp_driver();
        }

        // """Get drive motor
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     temp (float): temperature [degrees Celsius]
        // """
        public Tuple<int, float> get_temp_motor()
        {
            return this._get_temp_motor();
        }

        // """Get bus voltage
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     volt (float): volt [V]
        // """
        public Tuple<int, float> get_bus_volt()
        {
            return this._get_bus_volt();
        }

        // """Get bus current
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     current (float): current [A]
        // """
        public Tuple<int, float> get_bus_curr()
        {
            return this._get_bus_curr();
        }

        // """Get error code, the meaning of the fault code is referred to the appendix <fault code>
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     code (int): error code
        // """
        public Tuple<int, int> get_error_code()
        {
            return this._get_error_code();
        }
    }
}
