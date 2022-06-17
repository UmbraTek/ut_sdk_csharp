using System;
using utapi.common;

namespace utapi.basic
{
    class _ArmApiBase
    {
        private SocketFP socket_fp;

        private UtrcClient utrc_client;

        private UtrcType tx_data;

        private bool _is_err;

        private byte _AXIS;

        private ARM_REG reg;

        public void _init_(SocketFP _socket_fp)
        {
            _is_err = false;
            socket_fp = _socket_fp;
            utrc_client = new UtrcClient(socket_fp);
            tx_data = new UtrcType();
            tx_data.state = 0x00;
            tx_data.master_id = 0xAA;
            tx_data.slave_id = 0x55;
            _AXIS = 6;
            reg = new ARM_REG(_AXIS);

            Tuple<int, int> rets = get_axis();
            if (rets.Item1 == UTRC_RX_ERROR.STATE || rets.Item1 == 0)
            {
                _AXIS = (byte)(rets.Item2);
                reg = new ARM_REG(_AXIS);
            }
            else
            {
                Console.WriteLine("[UbotApi ] Error: __init__ get_axis, ret: " + rets.Item1.ToString());
                _is_err = true;
            }
        }

        public void close()
        {
            socket_fp.close();
            Console.WriteLine("[UbotApi ] ubot api close");
        }

        private bool _send(byte rw, byte[] cmd, byte[] cmd_data)
        {
            if (_is_err) return false;
            byte data_wlen = 0;
            if (rw == UTRC_RW.R)
            {
                data_wlen = cmd[1];
            }
            else
            {
                data_wlen = cmd[3];
            }
            tx_data.rw = rw;
            tx_data.cmd = cmd[0];
            tx_data.len = (byte)(data_wlen + 1);
            for (int i = 0; i < data_wlen; i++)
            {
                tx_data.data[i] = cmd_data[i];
            }
            utrc_client.send (tx_data);
            return true;
        }

        private int _pend(UtrcType rx_utrc, byte rw, byte[] cmd, int timeout_s = 1)
        {
            if (_is_err) return -999;
            byte data_rlen = 0;
            if (rw == UTRC_RW.R)
            {
                data_rlen = cmd[2];
            }
            else
            {
                data_rlen = cmd[4];
            }
            return utrc_client.pend(rx_utrc, tx_data, data_rlen, timeout_s);
        }

        public bool is_err()
        {
            return _is_err;
        }

        //############################################################
        //#                       Basic Api
        //############################################################
        // """Get the uuid of the arm
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     uuid (string): The unique code of umbratek products is also a certificate of repair and warranty
        //                    17-bit string
        // """
        public Tuple<int, String> get_uuid()
        {
            _send(UTRC_RW.R, reg.UUID, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.UUID);
            String uuid = "";
            for (int i = 0; i < 17; i++)
            {
                uuid = uuid + Convert.ToChar(utrc_rmsg.data[i]);
            }
            return Tuple.Create(ret, uuid);
        }

        // """Get the software version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     version (string): Software version, 20-bit string
        // """
        public Tuple<int, String> get_sw_version()
        {
            _send(UTRC_RW.R, reg.SW_VERSION, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.SW_VERSION);
            String version = "";
            for (int i = 0; i < 20; i++)
            {
                version = version + Convert.ToChar(utrc_rmsg.data[i]);
            }
            return Tuple.Create(ret, version);
        }

        // """Get the hardware version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     version (string): Hardware version, 20-bit string
        // """
        public Tuple<int, String> get_hw_version()
        {
            _send(UTRC_RW.R, reg.HW_VERSION, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.HW_VERSION);
            String version = "";
            for (int i = 0; i < 20; i++)
            {
                version = version + Convert.ToChar(utrc_rmsg.data[i]);
            }
            return Tuple.Create(ret, version);
        }

        // """Get the number of arm axes
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     axis (int): The number of arm axes
        // """
        public Tuple<int, int> get_axis()
        {
            _send(UTRC_RW.R, reg.UBOT_AXIS, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.UBOT_AXIS, 1);
            int axis = (int)(utrc_rmsg.data[0]);
            return Tuple.Create(ret, axis);
        }

        // """Power off the controller
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int shutdown_system()
        {
            byte[] txdata = new byte[1] { reg.SYS_SHUTDOWN[0] };
            _send(UTRC_RW.W, reg.SYS_SHUTDOWN, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.SYS_SHUTDOWN);
            return ret;
        }

        // """Reset the error state of the device
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int reset_err()
        {
            byte[] txdata = new byte[1] { reg.RESET_ERR[0] };
            _send(UTRC_RW.W, reg.RESET_ERR, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.RESET_ERR);
            return ret;
        }

        // """Restart the system
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int reboot_system()
        {
            byte[] txdata = new byte[1] { reg.SYS_REBOOT[0] };
            _send(UTRC_RW.W, reg.SYS_REBOOT, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.SYS_REBOOT);
            return ret;
        }

        // """Restore the parameters to factory settings
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int erase_parm()
        {
            byte[] txdata = new byte[1] { reg.ERASE_PARM[0] };
            _send(UTRC_RW.W, reg.ERASE_PARM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.ERASE_PARM);
            return ret;
        }

        // """Save the current parameter settings
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int saved_parm()
        {
            byte[] txdata = new byte[1] { reg.SAVED_PARM[0] };
            _send(UTRC_RW.W, reg.SAVED_PARM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.SAVED_PARM);
            return ret;
        }

        // ############################################################
        // #                       Control Api
        // ############################################################
        // Get the operating mode of the arm
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code
        // meaning mode (int): operating mode of the arm 0: position control mode 1:
        // servo motion mode, users must set to this mode first before using the
        // moveto_servoj interface. 2: joint teaching mode 3: cartesian teaching mode
        // (NOT used in current version)
        public Tuple<int, int> get_motion_mode()
        {
            _send(UTRC_RW.R, reg.MOTION_MDOE, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.MOTION_MDOE, 1);
            int mode = (int)(utrc_rmsg.data[0]);
            return Tuple.Create(ret, mode);
        }

        // """Set the operating mode of the arm
        // Args:
        //     mode (int): operating mode of the arm
        //         See the get_motion_mode
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_motion_mode(int mode)
        {
            byte[] txdata = new byte[1] { (byte)(mode) };
            _send(UTRC_RW.W, reg.MOTION_MDOE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOTION_MDOE);
            return ret;
        }

        // """Get the enable state of the arm
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     value (uint): There are a total of 32 bits, the 0th bit represents the enable state of the first joint brake, and so on.
        //         0xFFFF means all enable
        //         0x0000 means all disable
        //         0x0001 means only the first joint is enabled
        // """
        public Tuple<int, uint> get_motion_enable()
        {
            _send(UTRC_RW.R, reg.MOTION_ENABLE, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.MOTION_ENABLE);
            uint value = HexData.bytes_to_uint32_big(utrc_rmsg.data);
            return Tuple.Create(ret, value);
        }

        // """Set the enable state of the arm
        // Args:
        //     axis (int): Joint axis, if it is greater than the maximum number of joints, set all joints
        //     en (int): 1-enable, 0-disable
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_motion_enable(int axis, int en)
        {
            byte[] txdata = new byte[2] { (byte)(axis), (byte)(en) };
            _send(UTRC_RW.W, reg.MOTION_ENABLE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOTION_ENABLE);
            return ret;
        }

        // """Get the enable state of the joint brake
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     value (uint): There are a total of 32 bits, the 0th bit represents the enable state of the first joint brake, and so on.
        //         0xFFFF means all enable
        //         0x0000 means all disable
        //         0x0001 means only the first joint is enabled
        // """
        public Tuple<int, uint> get_brake_enable()
        {
            _send(UTRC_RW.R, reg.BRAKE_ENABLE, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.BRAKE_ENABLE);
            uint value = HexData.bytes_to_uint32_big(utrc_rmsg.data);
            return Tuple.Create(ret, value);
        }

        // """Only set the enable state of the joint brake
        // Args:
        //     axis (int): Joint axis, if it is greater than the maximum number of joints, set all joints
        //     en (int): 1-enable, 0-disable
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_brake_enable(int axis, int en)
        {
            byte[] txdata = new byte[2] { (byte)(axis), (byte)(en) };
            _send(UTRC_RW.W, reg.BRAKE_ENABLE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.BRAKE_ENABLE);
            return ret;
        }

        // """Get error code
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     code[0] (int): code[0] is error code,
        //     code[1] (int): code[1] is warning code
        // """
        public Tuple<int, int, int> get_error_code()
        {
            _send(UTRC_RW.R, reg.ERROR_CODE, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.ERROR_CODE);
            int code1 = (int)(utrc_rmsg.data[0]);
            int code2 = (int)(utrc_rmsg.data[1]);
            return Tuple.Create(ret, code1, code2);
        }

        // """Get servo status information
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     msg (String): Servo motor communication status and operation error code
        //         msg[0:Axis] Servo communication status
        //         msg[Axis:2*Axis] Servo error code
        // """
        public Tuple<int, String> get_servo_msg()
        {
            _send(UTRC_RW.R, reg.SERVO_MSG, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.SERVO_MSG);
            String msg = "";
            for (int i = 0; i < this._AXIS * 2; i++)
            {
                msg = msg + (Convert.ToInt32(utrc_rmsg.data[i])).ToString() + " ";
            }
            return Tuple.Create(ret, msg);
        }

        // """Get the running status of the arm
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     state (int): Current running status
        //         1: Moving
        //         2: Sleeping, ready to motion
        //         3: Paused
        //         4: Stopping
        // """
        public Tuple<int, int> get_motion_status()
        {
            _send(UTRC_RW.R, reg.MOTION_STATUS, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.MOTION_STATUS);
            int value = (int)(utrc_rmsg.data[0]);
            return Tuple.Create(ret, value);
        }

        // """Set the running status of the arm
        // Args:
        //     state (int): running status
        //         0: Set to ready
        //         3: Set to pause
        //         4: Set to stop
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_motion_status(int state)
        {
            byte[] txdata = new byte[1] { (byte)(state) };
            _send(UTRC_RW.W, reg.MOTION_STATUS, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOTION_STATUS);
            return ret;
        }

        // """Get the current number of instruction cache
        // Returns:
        //     ret (uint): Function execution result code, refer to appendix for code meaning
        // """
        public Tuple<int, uint> get_cmd_num()
        {
            _send(UTRC_RW.R, reg.CMD_NUM, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.CMD_NUM);
            uint value = HexData.bytes_to_uint32_big(utrc_rmsg.data);
            return Tuple.Create(ret, value);
        }

        // """Clear the current instruction cache
        // Args:
        //     value (uint): NOT used in current version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_cmd_num(uint value)
        {
            byte[] txdata = new byte[4] { 0, 0, 0, 0 };
            HexData.uint32_to_bytes_big (value, txdata);
            _send(UTRC_RW.W, reg.CMD_NUM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.CMD_NUM);
            return ret;
        }

        // ############################################################
        // #                     Trajectory Api
        // ############################################################
        // """Move to position (linear in tool-space)
        // Args:
        //     mvpose (list): cartesian position [mm mm mm rad rad rad]
        //     mvvelo (float): tool speed [mm/s]
        //     mvacc (float): tool acceleration [mm/sˆ2]
        //     mvtime (float): NOT used in current version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int moveto_cartesian_line(float[] pose, float mvvelo, float mvacc, float mvtime)
        {
            float[] txdata = new float[9] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
            for (int i = 0; i < 6; i++)
            {
                txdata[i] = pose[i];
            }
            txdata[6] = mvvelo;
            txdata[7] = mvacc;
            txdata[8] = mvtime;
            byte[] datas = new byte[9 * 4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.MOVET_LINE, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOVET_LINE);
            return ret;
        }

        // """Blend circular (in tool-space) and move linear (in tool-space) to position.
        // Accelerates to and moves with constant tool speed v.
        // Args:
        //     mvpose (list): cartesian position [mm mm mm rad rad rad]
        //     mvvelo (float): tool speed [mm/s]
        //     mvacc (float): tool acceleration [mm/sˆ2]
        //     mvtime (float): NOT used in current version
        //     mvradii (float): blend radius [mm]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int moveto_cartesian_lineb(float[] pose, float mvvelo, float mvacc, float mvtime, float mvradii)
        {
            float[] txdata = new float[10] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
            for (int i = 0; i < 6; i++)
            {
                txdata[i] = pose[i];
            }
            txdata[6] = mvvelo;
            txdata[7] = mvacc;
            txdata[8] = mvtime;
            txdata[9] = mvradii;
            byte[] datas = new byte[10 * 4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.MOVET_LINEB, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOVET_LINEB);
            return ret;
        }

        // """Move to position (circular in tool-space).
        // TCP moves on the circular arc segment from current pose, through pose1 to pose2.
        // Accelerates to and moves with constant tool speed mvvelo.
        // Args:
        //     pose1 (list): path cartesian position 1 [mm mm mm rad rad rad]
        //     pose2 (list): path cartesian position 2 [mm mm mm rad rad rad]
        //     mvvelo (float): tool speed [m/s]
        //     mvacc (float): tool acceleration [mm/sˆ2]
        //     mvtime (float): NOT used in current version
        //     percent (float): The length of the trajectory, the unit is a percentage of the circumference,
        //                       which can be tens of percent or hundreds of percent
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int moveto_cartesian_circle(float[] pose1, float[] pose2, float mvvelo, float mvacc, float mvtime, float percent)
        {
            float[] txdata = new float[16] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
            for (int i = 0; i < 6; i++)
            {
                txdata[i] = pose1[i];
            }
            for (int i = 0; i < 6; i++)
            {
                txdata[6 + i] = pose2[i];
            }
            txdata[12] = mvvelo;
            txdata[13] = mvacc;
            txdata[14] = mvtime;
            txdata[15] = percent;
            byte[] datas = new byte[16 * 4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.MOVET_CIRCLE, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOVET_CIRCLE);
            return ret;
        }

        // """Move to position (linear in joint-space) When using this command, the robot must be at a standstill
        // Args:
        //     mvjoint (list): target joint positions [rad]
        //     mvvelo (float): joint speed of leading axis [rad/s]
        //     mvacc (float): joint acceleration of leading axis [rad/sˆ2]
        //     mvtime (float): NOT used in current version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int moveto_joint_p2p(float[] mvjoint, float mvvelo, float mvacc, float mvtime)
        {
            float[] txdata = new float[this._AXIS + 3];
            for (int i = 0; i < this._AXIS; i++)
            {
                txdata[i] = mvjoint[i];
            }

            txdata[this._AXIS] = mvvelo;
            txdata[this._AXIS + 1] = mvacc;
            txdata[this._AXIS + 2] = mvtime;
            byte[] datas = new byte[(this._AXIS + 3) * 4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.MOVEJ_P2P, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOVEJ_P2P);
            return ret;
        }

        // """Move to position of home (linear in joint-space) When using this command, the robot must be at a standstill
        // Args:
        //     mvvelo (float): joint speed of leading axis [rad/s]
        //     mvacc (float): joint acceleration of leading axis [rad/sˆ2]
        //     mvtime (float): NOT used in current version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int moveto_home_p2p(float mvvelo, float mvacc, float mvtime)
        {
            float[] txdata = new float[3];
            txdata[0] = mvvelo;
            txdata[1] = mvacc;
            txdata[2] = mvtime;
            byte[] datas = new byte[12];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.MOVEJ_HOME, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOVEJ_HOME);
            return ret;
        }

        // """Servo to position (linear in joint-space)
        // Servo function used for online control of the robot
        // Args:
        //     mvjoint (list): joint positions [rad]
        //     mvvelo (float): NOT used in current version
        //     mvacc (float): NOT used in current version
        //     mvtime (float): NOT used in current version
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int moveto_servoj(float[] mvjoint, float mvvelo, float mvacc, float mvtime)
        {
            float[] txdata = new float[this._AXIS + 3];
            for (int i = 0; i < this._AXIS; i++)
            {
                txdata[i] = mvjoint[i];
            }

            txdata[this._AXIS] = mvvelo;
            txdata[this._AXIS + 1] = mvacc;
            txdata[this._AXIS + 2] = mvtime;
            byte[] datas = new byte[(this._AXIS + 3) * 4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.MOVE_SERVOJ, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOVE_SERVOJ);
            return ret;
        }

        // """Sleep for an amount of motion time
        // Args:
        //     time (float): sleep time [s]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int move_sleep(float time)
        {
            float[] txdata = new float[1] { time };
            byte[] datas = new byte[4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.MOVE_SLEEP, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.MOVE_SLEEP);
            return ret;
        }

        // """Sleep for an amount of plan time
        // Args:
        //     time (float): sleep time [s]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int plan_sleep(float time)
        {
            float[] txdata = new float[1] { time };
            byte[] datas = new byte[4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.PLAN_SLEEP, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.PLAN_SLEEP);
            return ret;
        }

        // ############################################################
        // #                    Parameter Api
        // ############################################################
        // """Get the jerk of the tool-space
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     jerk (float): jerk [mm/s^3]
        // """
        public Tuple<int, float> get_tcp_jerk()
        {
            _send(UTRC_RW.R, reg.TCP_JERK, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.TCP_JERK);
            float[] jerk = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, jerk);
            return Tuple.Create(ret, jerk[0]);
        }

        // """Set the jerk of the tool-space
        // Args:
        //     jerk (float): jerk [mm/s^3]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tcp_jerk(float jerk)
        {
            float[] txdata = new float[1] { jerk };
            byte[] datas = new byte[4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.TCP_JERK, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.TCP_JERK);
            return ret;
        }

        // """Set the maximum acceleration of the tool-space
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     maxacc (float): maximum acceleration [mm/s^2]
        // """
        public Tuple<int, float> get_tcp_maxacc()
        {
            _send(UTRC_RW.R, reg.TCP_MAXACC, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.TCP_MAXACC);
            float[] maxacc = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, maxacc);
            return Tuple.Create(ret, maxacc[0]);
        }

        // """Set the maximum acceleration of the tool-space
        // Args:
        //     maxacc (float): maximum acceleration [mm/s^2]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tcp_maxacc(float maxacc)
        {
            float[] txdata = new float[1] { maxacc };
            byte[] datas = new byte[4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.TCP_MAXACC, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.TCP_MAXACC);
            return ret;
        }

        // """Get the jerk of the joint-space
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     jerk (float): jerk [rad/s^3]
        // """
        public Tuple<int, float> get_joint_jerk()
        {
            _send(UTRC_RW.R, reg.JOINT_JERK, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.JOINT_JERK);

            float[] jerk = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, jerk);
            return Tuple.Create(ret, jerk[0]);
        }

        // """Set the jerk of the joint-space
        // Args:
        //     jerk (float): jerk [rad/s^3]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_joint_jerk(float jerk)
        {
            float[] txdata = new float[1] { jerk };
            byte[] datas = new byte[4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.JOINT_JERK, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.JOINT_JERK);
            return ret;
        }

        // """Get the maximum acceleration of the joint-space
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     maxacc (float): Maximum acceleration [rad/s^2]
        // """
        public Tuple<int, float> get_joint_maxacc()
        {
            _send(UTRC_RW.R, reg.JOINT_MAXACC, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.JOINT_MAXACC);

            float[] maxacc = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, maxacc);
            return Tuple.Create(ret, maxacc[0]);
        }

        // """Set the maximum acceleration of the joint-space
        // Args:
        //     maxacc (float): maximum acceleration [rad/s^2]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_joint_maxacc(float maxacc)
        {
            float[] txdata = new float[1] { maxacc };
            byte[] datas = new byte[4];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.JOINT_MAXACC, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.JOINT_MAXACC);
            return ret;
        }

        // """Get the coordinate offset of the end tcp tool
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     offset (list): Offset cartesian position [mm mm mm rad rad rad]
        // """
        public Tuple<int, float[]> get_tcp_offset()
        {
            _send(UTRC_RW.R, reg.TCP_OFFSET, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.TCP_OFFSET);

            float[] offset = new float[6];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, offset, 6);
            return Tuple.Create(ret, offset);
        }

        // """Set the coordinate offset of the end tcp tool
        // Args:
        //     offset (list): Offset cartesian position [mm mm mm rad rad rad]
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tcp_offset(float[] offset)
        {
            byte[] datas = new byte[4 * offset.Length];
            HexData.fp32_to_bytes_big (offset, datas);
            _send(UTRC_RW.W, reg.TCP_OFFSET, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.TCP_OFFSET);
            return ret;
        }

        // """Get payload mass and center of gravity
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     value (list): [Mass, CoGx, CoGy, CoGz], mass in kilograms, Center of Gravity in millimeter
        // """
        public Tuple<int, float[]> get_tcp_load()
        {
            _send(UTRC_RW.R, reg.LOAD_PARAM, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.LOAD_PARAM);

            float[] load = new float[4];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, load, 4);
            return Tuple.Create(ret, load);
        }

        // """Set payload mass and center of gravity
        // This function must be called, when the payload weight or weight distribution changes
        // when the robot picks up or puts down a heavy workpiece.
        // The dir is specified as a vector, [CoGx, CoGy, CoGz], displacement,from the toolmount.
        // Args:
        //     mass (float): mass in kilograms
        //     dir (list): Center of Gravity: [CoGx, CoGy, CoGz] in millimeter
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_tcp_load(float mass, float[] dir)
        {
            float[] txdata = new float[4];
            txdata[0] = mass;
            txdata[1] = dir[0];
            txdata[2] = dir[1];
            txdata[3] = dir[2];
            byte[] datas = new byte[16];
            HexData.fp32_to_bytes_big (txdata, datas);
            _send(UTRC_RW.W, reg.LOAD_PARAM, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.LOAD_PARAM);
            return ret;
        }

        // """Get the direction of the acceleration experienced by the robot
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     value (list): 3D vector, describing the direction of the gravity, relative to the base of the robot.
        // """
        public Tuple<int, float[]> get_gravity_dir()
        {
            _send(UTRC_RW.R, reg.GRAVITY_DIR, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.GRAVITY_DIR);

            float[] value = new float[3];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, value, 3);
            return Tuple.Create(ret, value);
        }

        // """Set the direction of the acceleration experienced by the robot. When the robot mounting is fixed,
        // this corresponds to an accleration of gaway from the earth’s centre
        // $ set_gravity_dir([0, 9.82*sin(theta), 9.82*cos(theta)]) // will set the acceleration for a robot
        // that is rotated ”theta” radians around the x-axis of the robot base coordinate system
        // Args:
        //     value (list): 3D vector, describing the direction of the gravity, relative to the base of the robot.
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_gravity_dir(float[] value)
        {
            byte[] datas = new byte[4 * value.Length];
            HexData.fp32_to_bytes_big (value, datas);
            _send(UTRC_RW.W, reg.GRAVITY_DIR, datas);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.GRAVITY_DIR);
            return ret;
        }

        // """Get the sensitivity of collision detection
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     num (int): 0-5
        // """
        public Tuple<int, int> get_collis_sens()
        {
            _send(UTRC_RW.R, reg.COLLIS_SENS, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.COLLIS_SENS);
            return Tuple.Create(ret, (int)(utrc_rmsg.data[0]));
        }

        // """Set the sensitivity of collision detection
        // Args:
        //     num (int): 0-5, 0 means close collision detection, sensitivity increases from 1 to 5,
        //     and 5 is the highest sensitivity
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_collis_sens(int num)
        {
            byte[] txdata = new byte[1] { (byte)(num) };
            _send(UTRC_RW.W, reg.COLLIS_SENS, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.COLLIS_SENS);
            return ret;
        }

        // """Get the sensitivity of freedrive
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     num (int): 1-5
        // """
        public Tuple<int, int> get_teach_sens()
        {
            _send(UTRC_RW.R, reg.TEACH_SENS, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.TEACH_SENS);
            return Tuple.Create(ret, (int)(utrc_rmsg.data[0]));
        }

        // """Set the sensitivity of freedrive
        // Args:
        //     num (int): 1-5, sensitivity increases from 1 to 5, and 5 is the highest sensitivity
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_teach_sens(int num)
        {
            byte[] txdata = new byte[1] { (byte)(num) };
            _send(UTRC_RW.W, reg.TEACH_SENS, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.TEACH_SENS);
            return ret;
        }

        // ############################################################
        // #                       State Api
        // ############################################################
        // """Get the current target tool pose
        // Get the 6d pose representing the tool position and orientation specified in the base frame.
        // The calculation of this pose is based on the current target joint positions.
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     pos (list): The current target TCP vector; ([X, Y, Z, Rx, Ry, Rz]) [mm mm mm rad rad rad]
        // """
        public Tuple<int, float[]> get_tcp_target_pos()
        {
            _send(UTRC_RW.R, reg.TCP_POS_CURR, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.TCP_POS_CURR);

            float[] pose = new float[6];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, pose, 6);
            return Tuple.Create(ret, pose);
        }

        // """Get the desired angular position of all joints
        // The angular target positions are expressed in radians and returned as a vector of length N.
        // Note that the output might differ from the output of get_joint_actual_pose(),
        // especially during cceleration and heavy loads.
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     joints (list): The current target joint angular position vector in rad
        // """
        public Tuple<int, float[]> get_joint_target_pos()
        {
            _send(UTRC_RW.R, reg.JOINT_POS_CURR, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg.JOINT_POS_CURR);

            float[] joints = new float[6];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, joints, this._AXIS);
            return Tuple.Create(ret, joints);
        }

        // ############################################################
        // #                       Rs485 Api
        // ############################################################
        // """Read the 8-bit register of the device through the utrc protocol
        // Communicate immediately, do not wait for the execution of other instructions in the queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        // Returns:
        //     value[0] (int): Function execution result code, refer to appendix for code meaning
        //     value[1] (int): Data
        // """
        public Tuple<int, int> get_utrc_int8_now(int line, int id, int reg)
        {
            byte[] txdata = new byte[3] { (byte)(line), (byte)(id), (byte)(reg) };
            _send(UTRC_RW.R, this.reg.UTRC_INT8_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, this.reg.UTRC_INT8_NOW);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return Tuple.Create((int)(utrc_rmsg.data[0]), (int)(utrc_rmsg.data[1]));
            }
            else
            {
                return Tuple.Create(ret, ret);
            }
        }

        // """Write the 8-bit register of the device through the utrc protocol
        // Communicate immediately, do not wait for the execution of other instructions in the queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        //     value (int): Data
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_utrc_int8_now(int line, int id, int reg)
        {
            byte[] txdata = new byte[3] { (byte)(line), (byte)(id), (byte)(reg) };
            _send(UTRC_RW.W, this.reg.UTRC_INT8_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, this.reg.UTRC_INT8_NOW);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return (int)(utrc_rmsg.data[0]);
            }
            else
            {
                return ret;
            }
        }

        // """Read the int32 register of the device through the utrc protocol
        // Communicate immediately, do not wait for the execution of other instructions in the queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        // Returns:
        //     value[0] (int): Function execution result code, refer to appendix for code meaning
        //     value[1] (int): Data
        // """
        public Tuple<int, int> get_utrc_int32_now(int line, int id, int reg)
        {
            byte[] txdata = new byte[3] { (byte)(line), (byte)(id), (byte)(reg) };
            _send(UTRC_RW.R, this.reg.UTRC_INT32_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, this.reg.UTRC_INT32_NOW);
            byte[] tem1 = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                tem1[i] = utrc_rmsg.data[i];
            }
            byte[] tem2 = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                tem2[i] = utrc_rmsg.data[i + 4];
            }
            int value1 = HexData.bytes_to_int32_big(tem1);
            int value2 = HexData.bytes_to_int32_big(tem2);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return Tuple.Create(value1, value2);
            }
            else
            {
                return Tuple.Create(ret, ret);
            }
        }

        // """Write the int32 register of the device through the utrc protocol
        // Communicate immediately, do not wait for the execution of other instructions in the queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        //     value (int): Data
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_utrc_int32_now(int line, int id, int reg, int value)
        {
            byte[] data = new byte[4];
            HexData.int32_to_bytes_big (value, data);
            byte[] txdata = new byte[7] { (byte)(line), (byte)(id), (byte)(reg), 0, 0, 0, 0 };
            for (int i = 0; i < 4; i++)
            {
                txdata[3 + i] = data[i];
            }
            _send(UTRC_RW.W, this.reg.UTRC_INT32_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, this.reg.UTRC_INT32_NOW);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return (int)(utrc_rmsg.data[0]);
            }
            else
            {
                return ret;
            }
        }

        // """Read the float register of the device through the utrc protocol
        // Communicate immediately, do not wait for the execution of other instructions in the queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        // Returns:
        //     value[0] (float): Function execution result code, refer to appendix for code meaning
        //     value[1] (float): Data
        // """
        public Tuple<float, float> get_utrc_float_now(int line, int id, int reg)
        {
            byte[] txdata = new byte[3] { (byte)(line), (byte)(id), (byte)(reg) };
            _send(UTRC_RW.R, this.reg.UTRC_FP32_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, this.reg.UTRC_FP32_NOW);
            byte[] tem1 = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                tem1[i] = utrc_rmsg.data[i];
            }
            byte[] tem2 = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                tem2[i] = utrc_rmsg.data[i + 4];
            }
            float[] value1 = new float[1];
            HexData.bytes_to_fp32_big (tem2, value1);
            float[] value2 = new float[1];
            HexData.bytes_to_fp32_big (tem2, value2);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return Tuple.Create(value1[0], value2[0]);
            }
            else
            {
                return Tuple.Create((float) ret, (float) ret);
            }
        }

        // """Write the float register of the device through the utrc protocol
        // Communicate immediately, do not wait for the execution of other instructions in the queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        //     value (float): Data
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_utrc_int32_now(int line, int id, int reg, float value)
        {
            byte[] data = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, data);
            byte[] txdata = new byte[7] { (byte)(line), (byte)(id), (byte)(reg), 0, 0, 0, 0 };
            for (int i = 0; i < 4; i++)
            {
                txdata[3 + i] = data[i];
            }
            _send(UTRC_RW.W, this.reg.UTRC_FP32_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, this.reg.UTRC_FP32_NOW);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return (int)(utrc_rmsg.data[0]);
            }
            else
            {
                return ret;
            }
        }

        // """Read the int8s register of the device through the utrc protocol
        // Communicate immediately, do not wait for the execution of other instructions in the queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        // Returns:
        //     value[0] (int): Function execution result code, refer to appendix for code meaning
        //     value[1] (list): Data
        // """
        public Tuple<int, byte[]> get_utrc_int8n_now(int line, int id, int reg, int len)
        {
            byte[] txdata = new byte[4] { (byte)(line), (byte)(id), (byte)(reg), (byte)(len) };
            this.reg.UTRC_INT8N_NOW[2] = (byte)(len + 1);
            _send(UTRC_RW.R, this.reg.UTRC_INT8N_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, this.reg.UTRC_INT8N_NOW);
            byte[] tem = new byte[len];
            for (int i = 0; i < len; i++)
            {
                tem[i] = utrc_rmsg.data[i + 1];
            }

            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return Tuple.Create((int) utrc_rmsg.data[0], tem);
            }
            else
            {
                return Tuple.Create(ret, new byte[1] { (byte) ret });
            }
        }

        // """Write the int8s register of the device through the utrc protocol
        // Communicate immediately, do not wait for the execution of other instructions in the queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        //     value (list): Data
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_utrc_int8n_now(int line, int id, int reg, int len, byte[] value)
        {
            byte[] txdata = new byte[3 + value.Length];
            txdata[0] = (byte) line;
            txdata[1] = (byte) id;
            txdata[2] = (byte) reg;
            txdata[3] = (byte) len;
            for (int i = 0; i < len; i++)
            {
                txdata[4 + i] = value[i];
            }
            this.reg.UTRC_INT8N_NOW[3] = (byte)(len + 4);
            _send(UTRC_RW.W, this.reg.UTRC_INT8N_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, this.reg.UTRC_INT8N_NOW);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return (int)(utrc_rmsg.data[0]);
            }
            else
            {
                return ret;
            }
        }

        // """Write the 8-bit register of the device through the utrc protocol
        // Queue communication, waiting for the completion of the execution of the instructions in the previous queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        //     value (int): Data
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_utrc_int8_que(int line, int id, int reg, int value)
        {
            byte[] txdata = new byte[4];
            txdata[0] = (byte) line;
            txdata[1] = (byte) id;
            txdata[2] = (byte) reg;
            txdata[3] = (byte) value;
            _send(UTRC_RW.W, this.reg.UTRC_INT8_QUE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, this.reg.UTRC_INT8_QUE);
            if (ret == UTRC_RX_ERROR.STATE)
            {
                return 0;
            }
            else
            {
                return ret;
            }
        }

        // """Write the int32 register of the device through the utrc protocol
        // Queue communication, waiting for the completion of the execution of the instructions in the previous queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        //     value (int): Data
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_utrc_int32_que(int line, int id, int reg, int value)
        {
            byte[] txdata = new byte[7];
            txdata[0] = (byte) line;
            txdata[1] = (byte) id;
            txdata[2] = (byte) reg;
            byte[] tem = new byte[4] { 0, 0, 0, 0 };
            HexData.int32_to_bytes_big (value, tem);
            for (int i = 0; i < 4; i++)
            {
                txdata[3 + i] = tem[i];
            }
            _send(UTRC_RW.W, this.reg.UTRC_INT32_QUE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, this.reg.UTRC_INT32_QUE);
            if (ret == UTRC_RX_ERROR.STATE)
            {
                return 0;
            }
            else
            {
                return ret;
            }
        }

        // """Write the float register of the device through the utrc protocol
        // Queue communication, waiting for the completion of the execution of the instructions in the previous queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        //     value (float): Data
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_utrc_float_que(int line, int id, int reg, float value)
        {
            byte[] txdata = new byte[7];
            txdata[0] = (byte) line;
            txdata[1] = (byte) id;
            txdata[2] = (byte) reg;
            byte[] data = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, data);
            for (int i = 0; i < 4; i++)
            {
                txdata[3 + i] = data[i];
            }
            _send(UTRC_RW.W, this.reg.UTRC_INT32_QUE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, this.reg.UTRC_INT32_QUE);
            if (ret == UTRC_RX_ERROR.STATE)
            {
                return 0;
            }
            else
            {
                return ret;
            }
        }

        // """Write the 8-bit register of the device through the utrc protocol
        // Queue communication, waiting for the completion of the execution of the instructions in the previous queue
        // Protocol details refer to [utrc_communication_protocol]
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     id (int): ID number of the device [1-125]
        //     reg (int): Device register address [0x01-0x7F]
        //     value (list): Data
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_utrc_int8n_que(int line, int id, int reg, int len, byte[] value)
        {
            byte[] txdata = new byte[4 + value.Length];
            txdata[0] = (byte) line;
            txdata[1] = (byte) id;
            txdata[2] = (byte) reg;
            txdata[3] = (byte) len;
            for (int i = 0; i < len; i++)
            {
                txdata[4 + i] = value[i];
            }
            this.reg.UTRC_INT8N_QUE[3] = (byte)(len + 4);
            _send(UTRC_RW.W, this.reg.UTRC_INT8N_QUE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, this.reg.UTRC_INT8N_QUE);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return (int)(utrc_rmsg.data[0]);
            }
            else
            {
                return ret;
            }
        }

        // """Send data to rs485 bus and receive data
        // Communicate immediately, do not wait for the execution of other instructions in the queue
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     timeout_ms (int): Receive data timeout [ms]
        //     tx_len (int): The length of the data sent
        //     rx_len (int): The length of the received data
        //     tx_data (list): Data sent
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        //     data (list): Data received
        // """
        public Tuple<int, byte[]> set_pass_rs485_now(int line, int timeout_ms, int tx_len, int rx_len, byte[] tx_data)
        {
            if (tx_len > 125 || rx_len > 125)
            {
                return Tuple.Create(-991, new byte[1] { 0 });
            }

            byte[] txdata = new byte[4 + tx_data.Length];
            txdata[0] = (byte) line;
            txdata[1] = (byte) timeout_ms;
            txdata[2] = (byte) tx_len;
            txdata[3] = (byte) rx_len;
            for (int i = 0; i < tx_len; i++)
            {
                txdata[4 + i] = tx_data[i];
            }
            reg.PASS_RS485_NOW[3] = (byte)(tx_len + 4);
            reg.PASS_RS485_NOW[4] = (byte)(rx_len + 2);
            _send(UTRC_RW.W, reg.PASS_RS485_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.PASS_RS485_NOW);
            byte[] tem = new byte[rx_len + 1];
            for (int i = 0; i < rx_len + 1; i++)
            {
                tem[i] = utrc_rmsg.data[i + 1];
            }
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                return Tuple.Create((int) utrc_rmsg.data[0], tem);
            }
            else
            {
                return Tuple.Create(ret, new byte[1] { 0 });
            }
        }

        // """Send data to rs485 bus
        // Queue communication, waiting for the completion of the execution of the instructions in the previous queue
        // Args:
        //     line (int): RS485 line
        //         2: RS485 at the end of the robotic arm
        //         3: RS485 for control box
        //     tx_len (int): The length of the data sent
        //     tx_data (list): Data sent
        // Returns:
        //     ret (int): Function execution result code, refer to appendix for code meaning
        // """
        public int set_pass_rs485_que(int line, int tx_len, byte[] value)
        {
            byte[] txdata = new byte[2 + value.Length];
            txdata[0] = (byte) line;
            txdata[1] = (byte) tx_len;
            for (int i = 0; i < tx_len; i++)
            {
                txdata[2 + i] = value[i];
            }
            reg.PASS_RS485_QUE[3] = (byte)(tx_len + 4);
            _send(UTRC_RW.W, reg.PASS_RS485_QUE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg.PASS_RS485_QUE);
            if (ret == UTRC_RX_ERROR.STATE)
            {
                return 0;
            }
            else
            {
                return ret;
            }
        }

        public Tuple<int,float,float> get_utrc_u8float_now(int line,int id,int reg,int num)
        {
            byte[] txdata = new byte[4]{(byte) line,(byte) id,(byte) reg,(byte) num};
            _send(UTRC_RW.R, this.reg.UTRC_U8FP32_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, this.reg.UTRC_U8FP32_NOW);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                float[] value1 = new float[1];
                byte[] value1_bytes = new byte[4]{utrc_rmsg.data[0],utrc_rmsg.data[1],utrc_rmsg.data[2],utrc_rmsg.data[3]};
                HexData.bytes_to_fp32_big(value1_bytes, value1);
                float[] value2 = new float[1];
                byte[] value2_bytes = new byte[4]{utrc_rmsg.data[4],utrc_rmsg.data[5],utrc_rmsg.data[6],utrc_rmsg.data[7]};
                HexData.bytes_to_fp32_big(value2_bytes, value2);
                return Tuple.Create(ret,value1[0],value2[0]);
            }
            else
            {
                return Tuple.Create(ret,0.0f,0.0f);
            }
        }

        public int set_utrc_u8float_now(int line,int id,int reg,int num,float value)
        {
            byte[] send_data = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, send_data);
            byte[] txdata = new byte[8]{(byte) line,(byte) id,(byte) reg,(byte) num,send_data[0],send_data[1],send_data[2],send_data[3]};
            _send(UTRC_RW.W, this.reg.UTRC_U8FP32_NOW, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, this.reg.UTRC_U8FP32_NOW);
            if (ret == 0 || ret == UTRC_RX_ERROR.STATE)
            {
                int ret_value = HexData.bytes_to_int8(utrc_rmsg.data[0]);
                return ret_value;
            }
            else
            {
                return ret;
            }
        }
    }

}
