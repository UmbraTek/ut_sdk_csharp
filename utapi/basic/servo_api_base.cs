using System;
using utapi.common;

namespace utapi.basic
{
    class _ServoApiBase
    {
        private SocketFP socket_fp;

        private UtrcClient bus_client;

        private UtrcType tx_data;

        private bool _is_err;

        private int id;

        private int virid;

        private byte[] CPOS_TARGET = { 0x60, 0, 0, 0, 0 }; // startId endId pos*Axis

        private byte[] CTAU_TARGET = { 0x61, 0, 0, 0, 0 }; // startId endId tau*Axis

        private byte[] CPOSTAU_TARGET = { 0x62, 0, 0, 0, 0 }; // startId endId (pos+tau)*Axis

        private byte[] SPOSTAU_CURRENT = { 0x68, 0, 8 + 1, 0, 0 }; // Gets the current position and torque of an actuator

        private byte[] CPOSTAU_CURRENT = { 0x69, 2, 8 + 1, 0, 0 }; // startId endId

        protected void _init_(SocketFP _socket_fp, UtrcClient bus_client, UtrcType tx_data)
        {
            this.socket_fp = _socket_fp;
            this.bus_client = bus_client;
            this.tx_data = tx_data;

            this._is_err = false;
            this.id = 1;
            this.virid = 1;
        }

        protected void _close()
        {
            this.socket_fp.close();
        }

        protected void _connect_to_id(int id, int virtual_id = 0)
        {
            this.id = id;
            this.virid = virtual_id;
            this.tx_data.id = (byte) id;
            this.tx_data.slave_id = (byte) id;
        }

        private void _send(byte rw, byte[] cmd, byte[] cmd_data, int len_tx = 0)
        {
            if (this._is_err) return;
            byte data_wlen = 0;
            if (rw == UTRC_RW.R)
            {
                data_wlen = cmd[1];
            }
            else
            {
                data_wlen = cmd[3];
            }
            if (len_tx != 0) data_wlen = (byte) len_tx;
            this.tx_data.rw = rw;
            this.tx_data.cmd = cmd[0];
            this.tx_data.len = (byte)(data_wlen + 1);

            for (int i = 0; i < data_wlen; i++)
            {
                this.tx_data.data[i] = cmd_data[i];
            }
            this.bus_client.send(this.tx_data);
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
            return this.bus_client.pend(rx_utrc, tx_data, data_rlen, timeout_s);
        }

        protected bool is_err()
        {
            return _is_err;
        }

        private float int_to_rad(int val)
        {
            return (float) val * 0.00001f;
        }

        private int rad_to_int(float val)
        {
            return (int)(val * 100000);
        }

        //############################################################
        //#                       Basic Function
        //############################################################
        protected Tuple<int, int> __get_reg_int32(byte[] reg)
        {
            _send(UTRC_RW.R, reg, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, reg);
            int value = HexData.bytes_to_int32_big(utrc_rmsg.data);
            return Tuple.Create(ret, value);
        }

        protected int __set_reg_int32(byte[] reg, int value)
        {
            byte[] txdata = new byte[4];
            HexData.int32_to_bytes_big (value, txdata);
            _send(UTRC_RW.W, reg, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, reg);
            return ret;
        }

        // ############################################################
        // #                       Basic Api
        // ############################################################
        protected Tuple<int, String> _get_uuid()
        {
            _send(UTRC_RW.R, SERVO_REG.UUID, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.UUID);
            String uuid = "";
            for (int i = 0; i < 12; i++)
            {
                uuid = uuid + utrc_rmsg.data[i].ToString("X2");
            }
            return Tuple.Create(ret, uuid);
        }

        protected Tuple<int, String> _get_sw_version()
        {
            _send(UTRC_RW.R, SERVO_REG.SW_VERSION, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.SW_VERSION);
            String version = "";
            for (int i = 0; i < 12; i++)
            {
                version = version + Convert.ToChar(utrc_rmsg.data[i]);
            }
            return Tuple.Create(ret, version);
        }

        protected Tuple<int, String> _get_hw_version()
        {
            _send(UTRC_RW.R, SERVO_REG.HW_VERSION, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.HW_VERSION);
            String version = "";
            for (int i = 0; i < 12; i++)
            {
                version = version + utrc_rmsg.data[i].ToString("X2");
            }
            return Tuple.Create(ret, version);
        }

        protected Tuple<int, String> _get_multi_version()
        {
            _send(UTRC_RW.R, SERVO_REG.MULTI_VERSION, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.MULTI_VERSION);
            uint ver = HexData.bytes_to_uint32_big(utrc_rmsg.data);
            String version_tem = "0000000000" + ver.ToString();
            String version = version_tem.Substring(version_tem.Length - 12);
            return Tuple.Create(ret, version);
        }

        protected Tuple<int, float> _get_mech_ratio()
        {
            _send(UTRC_RW.R, SERVO_REG.MECH_RATIO, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.MECH_RATIO);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_mech_ratio(float ratio)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { ratio };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.MECH_RATIO, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.MECH_RATIO);
            return ret;
        }

        protected int _set_com_id(int id)
        {
            byte[] txdata = new byte[1] { (byte) id };
            _send(UTRC_RW.W, SERVO_REG.COM_ID, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.COM_ID);
            return ret;
        }

        protected int _set_com_baud(int baud)
        {
            byte[] txdata = new byte[4];
            HexData.int32_to_bytes_big (baud, txdata);
            _send(UTRC_RW.W, SERVO_REG.COM_BAUD, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.COM_BAUD);
            return ret;
        }

        protected int _reset_err()
        {
            byte[] txdata = new byte[1] { SERVO_REG.RESET_ERR[0] };
            _send(UTRC_RW.W, SERVO_REG.RESET_ERR, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.RESET_ERR);
            return ret;
        }

        protected int _restart_driver()
        {
            byte[] txdata = new byte[1] { SERVO_REG.REBOOT_DRIVER[0] };
            _send(UTRC_RW.W, SERVO_REG.REBOOT_DRIVER, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.REBOOT_DRIVER);
            return ret;
        }

        protected int _erase_parm()
        {
            byte[] txdata = new byte[1] { SERVO_REG.ERASE_PARM[0] };
            _send(UTRC_RW.W, SERVO_REG.ERASE_PARM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.ERASE_PARM, 3);
            return ret;
        }

        protected int _saved_parm()
        {
            byte[] txdata = new byte[1] { SERVO_REG.SAVED_PARM[0] };
            _send(UTRC_RW.W, SERVO_REG.SAVED_PARM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.SAVED_PARM, 3);
            return ret;
        }

        // ############################################################
        // #                       Ectension Api
        // ############################################################
        protected Tuple<int, float> _get_elec_ratio()
        {
            _send(UTRC_RW.R, SERVO_REG.ELEC_RATIO, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.ELEC_RATIO);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_elec_ratio(float ratio)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { ratio };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.ELEC_RATIO, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.ELEC_RATIO);
            return ret;
        }

        protected Tuple<int, int> _get_motion_dir()
        {
            _send(UTRC_RW.R, SERVO_REG.MOTION_DIR, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.MOTION_DIR);
            return Tuple.Create(ret, HexData.bytes_to_int8(utrc_rmsg.data[0]));
        }

        protected int _set_motion_dir(int dir)
        {
            byte[] txdata = new byte[1] { (byte) dir };
            _send(UTRC_RW.W, SERVO_REG.MOTION_DIR, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.MOTION_DIR);
            return ret;
        }

        protected Tuple<int, int> _get_iwdg_cyc()
        {
            return __get_reg_int32(SERVO_REG.IWDG_CYC);
        }

        protected int _set_iwdg_cyc(int cyc)
        {
            return __set_reg_int32(SERVO_REG.IWDG_CYC, cyc);
        }

        protected Tuple<int, int, int> _get_temp_limit()
        {
            _send(UTRC_RW.R, SERVO_REG.TEMP_LIMIT, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TEMP_LIMIT);
            int min = HexData.bytes_to_int8(utrc_rmsg.data[0]);
            int max = HexData.bytes_to_int8(utrc_rmsg.data[1]);
            return Tuple.Create(ret, min, max);
        }

        protected int _set_temp_limit(int min, int max)
        {
            byte[] txdata = new byte[2] { (byte) min, (byte) max };
            _send(UTRC_RW.W, SERVO_REG.TEMP_LIMIT, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.TEMP_LIMIT);
            return ret;
        }

        protected Tuple<int, int, int> _get_volt_limit()
        {
            _send(UTRC_RW.R, SERVO_REG.VOLT_LIMIT, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.VOLT_LIMIT);
            int min = HexData.bytes_to_int8(utrc_rmsg.data[0]);
            int max = HexData.bytes_to_int8(utrc_rmsg.data[1]);
            return Tuple.Create(ret, min, max);
        }

        protected int _set_volt_limit(int min, int max)
        {
            byte[] txdata = new byte[2] { (byte) min, (byte) max };
            _send(UTRC_RW.W, SERVO_REG.VOLT_LIMIT, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.VOLT_LIMIT);
            return ret;
        }

        protected Tuple<int, float> _get_curr_limit()
        {
            _send(UTRC_RW.R, SERVO_REG.CURR_LIMIT, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.CURR_LIMIT);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_curr_limit(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.CURR_LIMIT, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.CURR_LIMIT);
            return ret;
        }

        protected Tuple<int, int> _get_brake_pwm()
        {
            _send(UTRC_RW.R, SERVO_REG.BRAKE_PWM, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.BRAKE_PWM);
            return Tuple.Create(ret, HexData.bytes_to_int8(utrc_rmsg.data[0]));
        }

        protected int _set_brake_pwm(int brake_pwm)
        {
            byte[] txdata = new byte[1] { (byte) brake_pwm };
            _send(UTRC_RW.W, SERVO_REG.BRAKE_PWM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.BRAKE_PWM);
            return ret;
        }

        // ############################################################
        // #                       Control Api
        // ############################################################
        protected Tuple<int, int> _get_motion_mode()
        {
            _send(UTRC_RW.R, SERVO_REG.MOTION_MDOE, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.MOTION_MDOE);
            return Tuple.Create(ret, (int) utrc_rmsg.data[0]);
        }

        protected int _set_motion_mode(int mode)
        {
            byte[] txdata = new byte[1] { (byte) mode };
            _send(UTRC_RW.W, SERVO_REG.MOTION_MDOE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.MOTION_MDOE);
            return ret;
        }

        protected Tuple<int, int> _get_motion_enable()
        {
            _send(UTRC_RW.R, SERVO_REG.MOTION_ENABLE, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.MOTION_ENABLE);
            return Tuple.Create(ret, (int) utrc_rmsg.data[0]);
        }

        protected int _set_motion_enable(int mode)
        {
            byte[] txdata = new byte[1] { (byte) mode };
            _send(UTRC_RW.W, SERVO_REG.MOTION_ENABLE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.MOTION_ENABLE);
            return ret;
        }

        protected Tuple<int, int> _get_brake_enable()
        {
            _send(UTRC_RW.R, SERVO_REG.BRAKE_ENABLE, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.BRAKE_ENABLE);
            return Tuple.Create(ret, (int) utrc_rmsg.data[0]);
        }

        protected int _set_brake_enable(int able)
        {
            byte[] txdata = new byte[1] { (byte) able };
            _send(UTRC_RW.W, SERVO_REG.BRAKE_ENABLE, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.BRAKE_ENABLE);
            return ret;
        }

        protected Tuple<int, float> _get_temp_driver()
        {
            _send(UTRC_RW.R, SERVO_REG.TEMP_DRIVER, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TEMP_DRIVER);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected Tuple<int, float> _get_temp_motor()
        {
            _send(UTRC_RW.R, SERVO_REG.TEMP_MOTOR, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TEMP_MOTOR);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected Tuple<int, float> _get_bus_volt()
        {
            _send(UTRC_RW.R, SERVO_REG.BUS_VOLT, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.BUS_VOLT);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected Tuple<int, float> _get_bus_curr()
        {
            _send(UTRC_RW.R, SERVO_REG.BUS_CURR, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.BUS_CURR);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected Tuple<int, float> _get_multi_volt()
        {
            _send(UTRC_RW.R, SERVO_REG.MULTI_VOLT, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.MULTI_VOLT);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected Tuple<int, int> _get_error_code()
        {
            _send(UTRC_RW.R, SERVO_REG.ERROR_CODE, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.ERROR_CODE);
            return Tuple.Create(ret, HexData.bytes_to_int8(utrc_rmsg.data[0]));
        }

        // ############################################################
        // #                       Position Api
        // ############################################################
        protected Tuple<int, float> _get_pos_target()
        {
            _send(UTRC_RW.R, SERVO_REG.POS_TARGET, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_TARGET);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_pos_target(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.POS_TARGET, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_TARGET);
            return ret;
        }

        protected Tuple<int, float> _get_pos_current()
        {
            _send(UTRC_RW.R, SERVO_REG.POS_CURRENT, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_CURRENT);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected Tuple<int, float> _get_pos_limit_min()
        {
            _send(UTRC_RW.R, SERVO_REG.POS_LIMIT_MIN, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_LIMIT_MIN);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_pos_limit_min(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.POS_LIMIT_MIN, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_LIMIT_MIN);
            return ret;
        }

        protected Tuple<int, float> _get_pos_limit_max()
        {
            _send(UTRC_RW.R, SERVO_REG.POS_LIMIT_MAX, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_LIMIT_MAX);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_pos_limit_max(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.POS_LIMIT_MAX, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_LIMIT_MAX);
            return ret;
        }

        protected Tuple<int, float> _get_pos_limit_diff()
        {
            _send(UTRC_RW.R, SERVO_REG.POS_LIMIT_DIFF, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_LIMIT_DIFF);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_pos_limit_diff(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.POS_LIMIT_DIFF, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_LIMIT_DIFF);
            return ret;
        }

        protected Tuple<int, float> _get_pos_pidp()
        {
            _send(UTRC_RW.R, SERVO_REG.POS_PIDP, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_PIDP);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_pos_pidp(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.POS_PIDP, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_PIDP);
            return ret;
        }

        protected Tuple<int, int> _get_pos_smooth_cyc()
        {
            _send(UTRC_RW.R, SERVO_REG.POS_SMOOTH_CYC, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_SMOOTH_CYC);
            return Tuple.Create(ret, HexData.bytes_to_int8(utrc_rmsg.data[0]));
        }

        protected int _set_pos_smooth_cyc(int value)
        {
            byte[] txdata = new byte[1] { (byte) value };
            _send(UTRC_RW.W, SERVO_REG.POS_SMOOTH_CYC, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_SMOOTH_CYC);
            return ret;
        }

        protected Tuple<int, float> _get_pos_adrc_param(int i)
        {
            byte[] txdata = new byte[1] { (byte) i };
            _send(UTRC_RW.R, SERVO_REG.POS_ADRC_PARAM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_ADRC_PARAM);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_pos_adrc_param(int i, float param)
        {
            byte[] txdata1 = new byte[4];
            float[] tem = new float[1] { param };
            HexData.fp32_to_bytes_big (tem, txdata1);
            byte[] txdata = new byte[5] { (byte) i, txdata1[0], txdata1[1], txdata1[2], txdata1[3] };
            _send(UTRC_RW.W, SERVO_REG.POS_ADRC_PARAM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_ADRC_PARAM);
            return ret;
        }

        protected int _pos_cal_zero(int value)
        {
            byte[] txdata = new byte[1] { (byte) SERVO_REG.POS_CAL_ZERO[0] };
            _send(UTRC_RW.W, SERVO_REG.POS_CAL_ZERO, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_CAL_ZERO);
            return ret;
        }

        // ############################################################
        // #                       Speed Api
        // ############################################################
        protected Tuple<int, float> _get_vel_target()
        {
            _send(UTRC_RW.R, SERVO_REG.VEL_TARGET, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.VEL_TARGET);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_vel_target(int value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.VEL_TARGET, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.VEL_TARGET);
            return ret;
        }

        protected Tuple<int, float> _get_vel_current()
        {
            _send(UTRC_RW.R, SERVO_REG.VEL_CURRENT, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.VEL_CURRENT);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected Tuple<int, float> _get_vel_limit_min()
        {
            _send(UTRC_RW.R, SERVO_REG.VEL_LIMIT_MIN, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.VEL_LIMIT_MIN);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_vel_limit_min(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.VEL_LIMIT_MIN, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.VEL_LIMIT_MIN);
            return ret;
        }

        protected Tuple<int, float> _get_vel_limit_max()
        {
            _send(UTRC_RW.R, SERVO_REG.VEL_LIMIT_MAX, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.VEL_LIMIT_MAX);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_vel_limit_max(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.VEL_LIMIT_MAX, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.VEL_LIMIT_MAX);
            return ret;
        }

        protected Tuple<int, float> _get_vel_limit_diff()
        {
            _send(UTRC_RW.R, SERVO_REG.VEL_LIMIT_DIFF, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.VEL_LIMIT_DIFF);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_vel_limit_diff(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.VEL_LIMIT_DIFF, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.VEL_LIMIT_DIFF);
            return ret;
        }

        protected Tuple<int, float> _get_vel_pidp()
        {
            _send(UTRC_RW.R, SERVO_REG.VEL_PIDP, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.VEL_PIDP);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_vel_pidp(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.VEL_PIDP, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.VEL_PIDP);
            return ret;
        }

        protected Tuple<int, float> _get_vel_pidi()
        {
            _send(UTRC_RW.R, SERVO_REG.VEL_PIDI, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.VEL_PIDI);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_vel_pidi(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.VEL_PIDI, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.VEL_PIDI);
            return ret;
        }

        protected Tuple<int, int> _get_vel_smooth_cyc()
        {
            _send(UTRC_RW.R, SERVO_REG.VEL_SMOOTH_CYC, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.VEL_SMOOTH_CYC);
            return Tuple.Create(ret, HexData.bytes_to_int8(utrc_rmsg.data[0]));
        }

        protected int _set_vel_smooth_cyc(int value)
        {
            byte[] txdata = new byte[1] { (byte) value };
            _send(UTRC_RW.W, SERVO_REG.VEL_SMOOTH_CYC, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.VEL_SMOOTH_CYC);
            return ret;
        }

        protected Tuple<int, float> _get_vel_adrc_param(int i)
        {
            byte[] txdata = new byte[1] { (byte) i };
            _send(UTRC_RW.R, SERVO_REG.POS_ADRC_PARAM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_ADRC_PARAM);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_vel_adrc_param(int i, float param)
        {
            byte[] txdata1 = new byte[4];
            float[] tem = new float[1] { param };
            HexData.fp32_to_bytes_big (tem, txdata1);
            byte[] txdata = new byte[5] { (byte) i, txdata1[0], txdata1[1], txdata1[2], txdata1[3] };
            _send(UTRC_RW.W, SERVO_REG.POS_ADRC_PARAM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_ADRC_PARAM);
            return ret;
        }

        // ############################################################
        // #                       Current Api
        // ############################################################
        protected Tuple<int, float> _get_tau_target()
        {
            _send(UTRC_RW.R, SERVO_REG.TAU_TARGET, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TAU_TARGET);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_tau_target(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.TAU_TARGET, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.TAU_TARGET);
            return ret;
        }

        protected Tuple<int, float> _get_tau_current()
        {
            _send(UTRC_RW.R, SERVO_REG.TAU_CURRENT, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TAU_CURRENT);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected Tuple<int, float> _get_tau_limit_min()
        {
            _send(UTRC_RW.R, SERVO_REG.TAU_LIMIT_MIN, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TAU_LIMIT_MIN);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_tau_limit_min(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.TAU_LIMIT_MIN, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.TAU_LIMIT_MIN);
            return ret;
        }

        protected Tuple<int, float> _get_tau_limit_max()
        {
            _send(UTRC_RW.R, SERVO_REG.TAU_LIMIT_MAX, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TAU_LIMIT_MAX);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_tau_limit_max(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.TAU_LIMIT_MAX, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.TAU_LIMIT_MAX);
            return ret;
        }

        protected Tuple<int, float> _get_tau_limit_diff()
        {
            _send(UTRC_RW.R, SERVO_REG.TAU_LIMIT_DIFF, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TAU_LIMIT_DIFF);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_tau_limit_diff(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.TAU_LIMIT_DIFF, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.TAU_LIMIT_DIFF);
            return ret;
        }

        protected Tuple<int, float> _get_tau_pidp()
        {
            _send(UTRC_RW.R, SERVO_REG.TAU_PIDP, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TAU_PIDP);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_tau_pidp(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.TAU_PIDP, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.TAU_PIDP);
            return ret;
        }

        protected Tuple<int, float> _get_tau_pidi()
        {
            _send(UTRC_RW.R, SERVO_REG.TAU_PIDI, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TAU_PIDI);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_tau_pidi(float value)
        {
            byte[] txdata = new byte[4];
            float[] tem = new float[1] { value };
            HexData.fp32_to_bytes_big (tem, txdata);
            _send(UTRC_RW.W, SERVO_REG.TAU_PIDI, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.TAU_PIDI);
            return ret;
        }

        protected Tuple<int, int> _get_tau_smooth_cyc()
        {
            _send(UTRC_RW.R, SERVO_REG.TAU_SMOOTH_CYC, null);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.TAU_SMOOTH_CYC);
            return Tuple.Create(ret, HexData.bytes_to_int8(utrc_rmsg.data[0]));
        }

        protected int _set_tau_smooth_cyc(int value)
        {
            byte[] txdata = new byte[1] { (byte) value };
            _send(UTRC_RW.W, SERVO_REG.TAU_SMOOTH_CYC, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.TAU_SMOOTH_CYC);
            return ret;
        }

        protected Tuple<int, float> _get_tau_adrc_param(int i)
        {
            byte[] txdata = new byte[1] { (byte) i };
            _send(UTRC_RW.R, SERVO_REG.POS_ADRC_PARAM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SERVO_REG.POS_ADRC_PARAM);
            float[] val = new float[1];
            HexData.bytes_to_fp32_big(utrc_rmsg.data, val);
            return Tuple.Create(ret, val[0]);
        }

        protected int _set_tau_adrc_param(int i, float param)
        {
            byte[] txdata1 = new byte[4];
            float[] tem = new float[1] { param };
            HexData.fp32_to_bytes_big (tem, txdata1);
            byte[] txdata = new byte[5] { (byte) i, txdata1[0], txdata1[1], txdata1[2], txdata1[3] };
            _send(UTRC_RW.W, SERVO_REG.POS_ADRC_PARAM, txdata);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.W, SERVO_REG.POS_ADRC_PARAM);
            return ret;
        }

        //############################################################
        //#                       Developer Api
        //############################################################
        protected int _set_cpos_target(int sid, int eid, float[] pos)
        {
            int id = this.id;
            _connect_to_id(0x55, 0x55);
            int num = eid - sid + 1;
            byte[] txdata = new byte[pos.Length * 4 + 2];
            txdata[0] = (byte) sid;
            txdata[1] = (byte) eid;
            CPOS_TARGET[3] = (byte)(2 + 4 * num);
            HexData.fp32_to_bytes_big(pos, txdata, 2);
            _send(UTRC_RW.W, CPOS_TARGET, txdata);
            _connect_to_id (id, id);
            return 0;
        }

        protected int _set_ctau_target(int sid, int eid, float[] tau)
        {
            int id = this.id;
            _connect_to_id(0x55, 0x55);
            int num = eid - sid + 1;
            byte[] txdata = new byte[tau.Length * 4 + 2];
            txdata[0] = (byte) sid;
            txdata[1] = (byte) eid;
            CTAU_TARGET[3] = (byte)(2 + 4 * num);
            HexData.fp32_to_bytes_big(tau, txdata, 2);
            _send(UTRC_RW.W, CTAU_TARGET, txdata);
            _connect_to_id (id, id);
            return 0;
        }

        protected int _set_cpostau_target(int sid, int eid, float[] pos, float[] tau)
        {
            int id = this.id;
            _connect_to_id(0x55, 0x55);
            int num = eid - sid + 1;
            float[] postau = new float[num * 2];
            for (int i = 0; i < num; i++)
            {
                postau[i * 2] = pos[i];
                postau[i * 2 + 1] = tau[i];
            }
            byte[] txdata = new byte[4 * num * 2 + 2];
            txdata[0] = (byte) sid;
            txdata[1] = (byte) eid;
            CPOSTAU_TARGET[3] = (byte)(4 * num * 2 + 2);
            HexData.fp32_to_bytes_big(postau, txdata, 2);
            _send(UTRC_RW.W, CPOSTAU_TARGET, txdata);
            _connect_to_id (id, id);
            return 0;
        }

        protected Tuple<int, int, float, float> _get_spostau_current()
        {
            _send(UTRC_RW.R, SPOSTAU_CURRENT, new byte[1]);
            UtrcType utrc_rmsg = new UtrcType();
            int ret = _pend(utrc_rmsg, UTRC_RW.R, SPOSTAU_CURRENT);
            int num = HexData.bytes_to_int8(utrc_rmsg.data[0]);
            float[] pos = new float[1];
            byte[] pos_bytes = new byte[4] { utrc_rmsg.data[1], utrc_rmsg.data[2], utrc_rmsg.data[3], utrc_rmsg.data[4] };
            HexData.bytes_to_fp32_big (pos_bytes, pos);
            float[] tau = new float[1];
            byte[] tau_bytes = new byte[4] { utrc_rmsg.data[5], utrc_rmsg.data[6], utrc_rmsg.data[7], utrc_rmsg.data[8] };
            HexData.bytes_to_fp32_big (tau_bytes, tau);
            return Tuple.Create(ret, num, pos[0], tau[0]);
        }

        protected Tuple<int[], int[], float[], float[]> _get_cpostau_current(int sid, int eid)
        {
            int id = this.id;
            int num = (eid - sid + 1);
            int[] ret = new int[num];
            int[] broadcast_num = new int[num];
            float[] pos = new float[num];
            float[] tau = new float[num];

            _connect_to_id(0x55, 0x55);
            byte[] txdata = new byte[2] { (byte) sid, (byte) eid };
            _send(UTRC_RW.R, CPOSTAU_CURRENT, txdata);
            for (int i = 0; i < num; i++)
            {
                UtrcType utrc_rmsg = new UtrcType();
                ret[i] = _pend(utrc_rmsg, UTRC_RW.R, CPOSTAU_CURRENT);
                if (ret[i] == UTRC_RX_ERROR.TIMEOUT)
                {
                    broadcast_num[i] = 0;
                    pos[i] = 0;
                    tau[i] = 0;
                }
                else
                {
                    broadcast_num[i] = HexData.bytes_to_int8(utrc_rmsg.data[0]);
                    float[] val = new float[2];
                    HexData.bytes_to_fp32_big(utrc_rmsg.data, val, 2, 1);
                    pos[i] = val[0];
                    tau[i] = val[1];
                }
            }
            _connect_to_id(id);
            return Tuple.Create(ret, broadcast_num, pos, tau);
        }
    }
}
