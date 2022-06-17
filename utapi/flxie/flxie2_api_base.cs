using System;
using utapi.basic;
using utapi.common;

namespace utapi.flxie
{
    class FlxiE2ApiBase : _ServoApiBase
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

        // """Get current limit threshold
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     value (float): Maximum current alarm threshold
        // """
        public Tuple<int, float> get_curr_limit()
        {
            return this._get_curr_limit();
        }

        // """Set current limit threshold
        // Args:
        //     value (float): Maximum current alarm threshold
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_curr_limit(float value)
        {
            return this._set_curr_limit(value);
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

        // ############################################################
        // #                       Position Api
        // ############################################################
        // """Get target position
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     pos (float): target position [rad]
        // """
        public Tuple<int, float> get_pos_target()
        {
            return this._get_pos_target();
        }

        // """Set target position
        // Args:
        //     pos (float): target position [rad]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_pos_target(float value)
        {
            return this._set_pos_target(value);
        }

        // """Get current position
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     pos (float): current position [rad]
        // """
        public Tuple<int, float> get_pos_current()
        {
            return this._get_pos_current();
        }

        // """Get the minimum limit threshold of the position in position mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     pos (float): position [rad]
        // """
        public Tuple<int, float> get_pos_limit_min()
        {
            return this._get_pos_limit_min();
        }

        // """Set the minimum limit threshold of the position in position mode,
        // other modes such as speed mode and current mode do not work
        // Args:
        //     pos (float): position [rad]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_pos_limit_min(float value)
        {
            return this._set_pos_limit_min(value);
        }

        // """Get the maximum limit threshold of the position in position mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     pos (float): position [rad]
        // """
        public Tuple<int, float> get_pos_limit_max()
        {
            return this._get_pos_limit_max();
        }

        // """Set the maximum limit threshold of the position in position mode,
        // other modes such as speed mode and current mode do not work
        // Args:
        //     pos (float): position [rad]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_pos_limit_max(float value)
        {
            return this._set_pos_limit_max(value);
        }

        // """Get position loop control parameter P
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     p (float): parameter P
        // """
        public Tuple<int, float> get_pos_pidp()
        {
            return this._get_pos_pidp();
        }

        // """Get position loop control parameter P
        // Args:
        //     p (float): parameter P
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_pos_pidp(float value)
        {
            return this._set_pos_pidp(value);
        }

        // """Get smoothing filter period of the position loop
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     cyc (int): smoothing period [1-125]
        // """
        public Tuple<int, int> get_pos_smooth_cyc()
        {
            return this._get_pos_smooth_cyc();
        }

        // """Set smoothing filter period of the position loop. The larger the smoothing period,
        // the smoother the movement and the slower the response. The range is 1 to 125
        // Args:
        //     cyc (int): smoothing period [1-125]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_pos_smooth_cyc(int value)
        {
            return this._set_pos_smooth_cyc(value);
        }

        // """Set current position as mechanical zero, after the operation, the user needs to restart the device
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int pos_cal_zero(int value)
        {
            return this._pos_cal_zero(value);
        }

        // ############################################################
        // #                       Current Api
        // ############################################################
        // """Get target current
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     tau (float): target current [A]
        // """
        public Tuple<int, float> get_tau_target()
        {
            return this._get_tau_target();
        }

        // """Set target current
        // Args:
        //     tau (float): target current [A]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tau_target(float value)
        {
            return this._set_tau_target(value);
        }

        // """Get current current
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     tau (float): current current [A]
        // """
        public Tuple<int, float> get_tau_current()
        {
            return this._get_tau_current();
        }

        // """Get the minimum limit threshold of the current
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     tau (float): current [A]
        // """
        public Tuple<int, float> get_tau_limit_min()
        {
            return this._get_tau_limit_min();
        }

        // """Set the minimum limit threshold of the current, all modes are effective
        // Args:
        //     tau (float): current [A]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tau_limit_min(float value)
        {
            return this._set_tau_limit_min(value);
        }

        // """Get the maximum limit threshold of the current
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     tau (float): current [A]
        // """
        public Tuple<int, float> get_tau_limit_max()
        {
            return this._get_tau_limit_max();
        }

        // """Set the maximum limit threshold of the current, all modes are effective
        // Args:
        //     tau (float): current [A]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tau_limit_max(float value)
        {
            return this._set_tau_limit_max(value);
        }

        // """Get current loop control parameter P
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     pid_p (float): parameter P
        // """
        public Tuple<int, float> get_tau_pidp()
        {
            return this._get_tau_pidp();
        }

        // """Set current loop control parameter P
        // Args:
        //     pid_p (float): parameter P
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tau_pidp(float value)
        {
            return this._set_tau_pidp(value);
        }

        // """Get current loop control parameter I
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     pid_i (float): parameter I
        // """
        public Tuple<int, float> get_tau_pidi()
        {
            return this._get_tau_pidi();
        }

        // """Set current loop control parameter I
        // Args:
        //     pid_i (float): parameter I
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tau_pidi(float value)
        {
            return this._set_tau_pidi(value);
        }

        // """Get smoothing filter period of the current loop
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     cyc (int): smoothing period [1-125]
        // """
        public Tuple<int, int> get_tau_smooth_cyc()
        {
            return this._get_tau_smooth_cyc();
        }

        // """Set smoothing filter period of the current loop. The larger the smoothing period,
        // the smoother the movement and the slower the response. The range is 1 to 125
        // Args:
        //     cyc (int): smoothing period [1-125]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tau_smooth_cyc(int value)
        {
            return this._set_tau_smooth_cyc(value);
        }
    }
}
