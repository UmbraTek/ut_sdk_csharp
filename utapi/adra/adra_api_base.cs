using System;
using utapi.basic;
using utapi.common;

namespace utapi.adra
{
    class AdraApiBase : _ServoApiBase
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

        // """Get the Multi-turn version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     version (int): Multi-turn version
        // """
        public Tuple<int, String> get_multi_version()
        {
            return this._get_multi_version();
        }

        // """Get the reduction ratio of the mechanical reducer
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     ratio (float): reduction ratio
        // """
        public Tuple<int, float> get_mech_ratio()
        {
            return this._get_mech_ratio();
        }

        // """Set the reduction ratio of the mechanical reducer
        // Args:
        //     ratio (float): reduction ratio
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_mech_ratio(float ratio)
        {
            return this._set_mech_ratio(ratio);
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
        // """Get electronic gear ratio
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     ratio (float): reduction ratio
        // """
        public Tuple<int, float> get_elec_ratio()
        {
            return this._get_elec_ratio();
        }

        // """Set electronic gear ratio
        // Args:
        //     ratio (float): reduction ratio
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_elec_ratio(float ratio)
        {
            return this._set_elec_ratio(ratio);
        }

        // """Get the direction of motion, 0: positive direction, 1: negative direction
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     dir (bool): 1 or 0
        // """
        public Tuple<int, int> get_motion_dir()
        {
            return this._get_motion_dir();
        }

        // """Set the direction of motion, 0: positive direction, 1: negative direction
        // Args:
        //     dir (bool): 1 or 0
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_motion_dir(int dir)
        {
            return this._set_motion_dir(dir);
        }

        //"""see the set_iwdg_cyc.
        //Returns:
        //    ret (int): Function execution result code, refer to appendix for code meaning.
        //    cyc (int): Cycle time.
        //"""
        public Tuple<int, int> get_iwdg_cyc()
        {
            return this._get_iwdg_cyc();
        }

        //  """Set the maximum interval of broadcast read commands. The unit of time is torque cycle.
        //  When reading actuator data in broadcast mode,
        //  you must send a broadcast read command within the specified period.
        //  If the communication interruption period exceeds the specified period, the actuator reports an error.
        //  If this function is not required, set it to 0 to disable it.
        //  Unit of period: 1 / CurrentCycle(Normal is 20 KHZ).
        //  For example, because the control cycle of the torque loop is 20KHz,
        //  if the communication detection cycle is set to 10000 and the actuator data is obtained through broadcast,
        //  the instruction of actuator data acquisition through broadcast must be continuously used,
        //  and the interval must be less than 0.5 seconds (10000/20khz).
        //  If the communication detection period is set to 0, the broadcast instruction can be used discontinuously.
        //  Args:
        //      cyc (int): Cycle time.
        //  Returns:
        //      ret (int): Function execution result code, refer to appendix for code meaning
        //  """
        public int set_iwdg_cyc(int cyc)
        {
            return this._set_iwdg_cyc(cyc);
        }

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

        public Tuple<int, int> get_brake_pwm()
        {
            return this._get_brake_pwm();
        }

        public int set_brake_pwm(int brake_pwm)
        {
            return this._set_brake_pwm(brake_pwm);
        }

        // ############################################################
        // #                       Control Api
        // ############################################################
        // """Get the operating mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     mode (int): operating mode of the arm
        //         1: Position mode
        //         2: Speed ​​mode
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
        //         2: Speed ​​mode
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

        // """Get brake enable status
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     enable (bool): 0 Disable brake, 1 Enable brake
        // """
        public Tuple<int, int> get_brake_enable()
        {
            return this._get_brake_enable();
        }

        // """Set the brake enable state, enable the brake separately, and operate this register only when the motion is disabled,
        // because the brake is automatically opened in the motion enable state.
        // Args:
        //     enable (bool): 0 Disable brake, 1 Enable brake
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_brake_enable(int able)
        {
            return this._set_brake_enable(able);
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

        // """Get battery voltage of multi-turn encoder
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     volt (float): volt [V]
        // """
        public Tuple<int, float> get_multi_volt()
        {
            return this._get_multi_volt();
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

        // """Get the maximum position following error threshold in position mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     pos (float): position [rad]
        // """
        public Tuple<int, float> get_pos_limit_diff()
        {
            return this._get_pos_limit_diff();
        }

        // """Set the maximum position following error threshold in position mode,
        // the tracking error alarm threshold of the current position and the target position,
        // other modes such as speed mode and current mode do not work
        // Args:
        //     pos (float): position [rad]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_pos_limit_diff(float value)
        {
            return this._set_pos_limit_diff(value);
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

        public Tuple<int, float> get_pos_adrc_param(int i)
        {
            //u"""Get speed loop ADRC parameters.
            //Args:
            //    i ([int]): Adrc has many parameters, which parameter needs to be get.
            //Returns:
            //    ret (int): Function execution result code, refer to appendix for code meaning.
            //    value (float): parameter Adrc.
            //"""
            return this._get_pos_adrc_param(i);
        }

        public int set_pos_adrc_param(int i, float param)
        {
            //u"""Set position loop ADRC parameters.
            //Args:
            //    i ([int]): Adrc has many parameters, which parameter needs to be set.
            //    param ([type]): [description]
            //Returns:
            //    ret (int): Function execution result code, refer to appendix for code meaning.
            //"""
            return this._set_pos_adrc_param(i, param);
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
        // #                       Speed Api
        // ############################################################
        // """Get target speed
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     vel (float): speed [rad/s]
        // """
        public Tuple<int, float> get_vel_target()
        {
            return this._get_vel_target();
        }

        // """Set target speed
        // Args:
        //     vel (float): speed [rad/s]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_vel_target(int value)
        {
            return this._set_vel_target(value);
        }

        // """Get current speed
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     vel (float): speed [rad/s]
        // """
        public Tuple<int, float> get_vel_current()
        {
            return this._get_vel_current();
        }

        // """Get the minimum limit of the speed in speed mode and position mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     vel (float): speed [rad/s]
        // """
        public Tuple<int, float> get_vel_limit_min()
        {
            return this._get_vel_limit_min();
        }

        // """Set the minimum limit of the speed in speed mode and position mode,
        // other modes such as current mode do not work
        // Args:
        //     vel (float): speed [rad/s]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_vel_limit_min(float value)
        {
            return this._set_vel_limit_min(value);
        }

        // """Get maximum limit of the speed in speed mode and position mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     vel (float): speed [rad/s]
        // """
        public Tuple<int, float> get_vel_limit_max()
        {
            return this._get_vel_limit_max();
        }

        // """Set maximum limit of the speed in speed mode and position mode,
        // other modes such as current mode do not work
        // Args:
        //     vel (float): speed [rad/s]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_vel_limit_max(float value)
        {
            return this._set_vel_limit_max(value);
        }

        // """Get the maximum speed following error threshold in the speed mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     vel (float): speed [rad/s]
        // """
        public Tuple<int, float> get_vel_limit_diff()
        {
            return this._get_vel_limit_diff();
        }

        // """Set the maximum speed following error threshold in the speed mode,
        // the tracking error alarm threshold of the current spped and the target speed,
        // other modes such as position mode and current mode do not work
        // Args:
        //     vel (float): speed [rad/s]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_vel_limit_diff(float value)
        {
            return this._set_vel_limit_diff(value);
        }

        // """Get speed loop control parameter P
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     p (float): parameter P
        // """
        public Tuple<int, float> get_vel_pidp()
        {
            return this._get_vel_pidp();
        }

        // """Set speed loop control parameter P
        // Args:
        //     p (float): parameter P
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_vel_pidp(float value)
        {
            return this._set_vel_pidp(value);
        }

        // """Get speed loop control parameter I
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     pid_i (float): parameter pid_i
        // """
        public Tuple<int, float> get_vel_pidi()
        {
            return this._get_vel_pidi();
        }

        // """Set speed loop control parameter I
        // Args:
        //     pid_i (float): parameter pid_i
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_vel_pidi(float value)
        {
            return this._set_vel_pidi(value);
        }

        // """Get smoothing filter period of the speed loop
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     cyc (int): smoothing period [1-125]
        // """
        public Tuple<int, int> get_vel_smooth_cyc()
        {
            return this._get_vel_smooth_cyc();
        }

        // """Set smoothing filter period of the speed loop. The larger the smoothing period,
        // the smoother the movement and the slower the response. The range is 1 to 125
        // Args:
        //     cyc (int): smoothing period [1-125]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_vel_smooth_cyc(int value)
        {
            return this._set_vel_smooth_cyc(value);
        }

        public Tuple<int, float> get_vel_adrc_param(int i)
        {
            //u"""Get speed loop ADRC parameters.
            //Args:
            //    i ([int]): Adrc has many parameters, which parameter needs to be get.
            //Returns:
            //    ret (int): Function execution result code, refer to appendix for code meaning.
            //    value (float): parameter Adrc.
            //"""
            return this._get_vel_adrc_param(i);
        }

        public int set_vel_adrc_param(int i, float param)
        {
            //u"""Set speed loop ADRC parameters.
            //Args:
            //    i ([int]): Adrc has many parameters, which parameter needs to be set.
            //    param ([type]): [description].
            //Returns:
            //    ret (int): Function execution result code, refer to appendix for code meaning.
            //"""
            return this._set_vel_adrc_param(i, param);
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

        // """Get the maximum current following error threshold in the current mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     tau (float): current [A]
        // """
        public Tuple<int, float> get_tau_limit_diff()
        {
            return this._get_tau_limit_diff();
        }

        // """Set the maximum current following error threshold in the current mode,
        // the tracking error alarm threshold of the current current and the target current,
        // other modes such as position mode and speed mode do not work
        // Args:
        //     tau (float): current [A]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tau_limit_diff(float value)
        {
            return this._set_tau_limit_diff(value);
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

        public Tuple<int, float> get_tau_adrc_param(int i)
        {
            //u"""Get torque loop ADRC parameters.
            //Args:
            //    i (int): Adrc has many parameters, which parameter needs to be get.
            //Returns:
            //    ret (int): Function execution result code, refer to appendix for code meaning.
            //    value (float): parameter Adrc.
            //"""
            return this._get_tau_adrc_param(i);
        }

        public int set_tau_adrc_param(int i, float param)
        {
            //u"""Set torque loop ADRC parameters.
            //Args:
            //    i (int): Adrc has many parameters, which parameter needs to be set.
            //    param (type): [description]
            //Returns:
            //    ret (int): Function execution result code, refer to appendix for code meaning.
            //"""
            return this._set_tau_adrc_param(i, param);
        }

        //############################################################
        //#                       Advanced Api
        //############################################################
        public int set_cpos_target(int sid, int eid, float[] pos)
        {
            //u"""Broadcast mode (one packet) sets multiple actuator target positions.
            //Args:
            //    sid (int): ID of the first actuator.
            //    eid (int): ID of the last actuator.
            //    pos (list): Target position of actuators, in ascending order of ID number.
            //Returns:
            //    ret (int): meaningless.
            //"""
            return this._set_cpos_target(sid, eid, pos);
        }

        public int set_ctau_target(int sid, int eid, float[] tau)
        {
            //u"""Broadcast mode (one packet) sets multiple actuator target torque.
            //Args:
            //    sid (int): ID of the first actuator.
            //    eid (int): ID of the last actuator.
            //    tau (list): Target torque of actuators, in ascending order of ID number.
            //Returns:
            //    ret (int): meaningless.
            //"""
            return this._set_ctau_target(sid, eid, tau);
        }

        public int set_cpostau_target(int sid, int eid, float[] pos, float[] tau)
        {
            //u"""Broadcast mode (one packet) sets multiple actuator target torque and feedforward torques.
            //Args:
            //    sid (int): ID of the first actuator.
            //    eid (int): ID of the last actuator.
            //    pos (list): Target position of actuators, in ascending order of ID number.
            //    tau (list): Feedforward torque of actuators, in ascending order of ID number.
            //Returns:
            //    ret (int): meaningless.
            //"""
            return this._set_cpostau_target(sid, eid, pos, tau);
        }

        public Tuple<int, int, float, float> get_spostau_current()
        {
            //u"""Gets the current position of the actuator, the current torque, and number of write broadcasts received.
            //At the same time, the number of received broadcast write commands is cleared to zero.

            //Args:

            //Returns:
            //    ret (int): Function execution result code, refer to appendix for code meaning.
            //    pos (int): Current position of actuators.
            //    tau (int): Current torque of actuators.
            //    num (int): Cnumber of write broadcasts received.
            //"""
            return this._get_spostau_current();
        }

        public Tuple<int[], int[], float[], float[]> get_cpostau_current(int sid, int eid)
        {
            //u"""Broadcast mode (one packet) gets multiple actuator current position,
            //current torque, and number of write broadcasts received.
            //At the same time, the number of received broadcast write commands is cleared to zero.

            //Args:
            //    sid (int): ID of the first actuator.
            //    eid (int): ID of the last actuator.

            //Returns:
            //    ret (list): Function execution result code, refer to appendix for code meaning,
            //                in ascending order of ID number.
            //    pos (list): Current position of actuators, in ascending order of ID number.
            //    tau (list): Current torque of actuators, in ascending order of ID number.
            //    num (list): Cnumber of write broadcasts received, in ascending order of ID number.
            //"""
            return this._get_cpostau_current(sid, eid);
        }
    }
}
