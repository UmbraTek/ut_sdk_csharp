namespace utapi.basic
{
    class RS485_LINE
    {
        public static byte SERVO = 1;

        public static byte TGPIO = 2;

        public static byte CGPIO = 3;
    }

    class ARM_REG
    {
        private byte Null;

        public byte AXIS;

        public byte[] UUID;

        public byte[] SW_VERSION;

        public byte[] HW_VERSION;

        public byte[] UBOT_AXIS;

        public byte[] SYS_SHUTDOWN;

        public byte[] RESET_ERR;

        public byte[] SYS_REBOOT;

        public byte[] ERASE_PARM;

        public byte[] SAVED_PARM;

        public byte[] MOTION_MDOE;

        public byte[] MOTION_ENABLE;

        public byte[] BRAKE_ENABLE;

        public byte[] ERROR_CODE;

        public byte[] SERVO_MSG;

        public byte[] MOTION_STATUS;

        public byte[] CMD_NUM;

        public byte[] MOVET_LINE;

        public byte[] MOVET_LINEB;

        public byte[] MOVET_CIRCLE;

        public byte[] MOVET_P2P;

        public byte[] MOVET_P2PB;

        public byte[] MOVEJ_LINE;

        public byte[] MOVEJ_LINEB;

        public byte[] MOVEJ_CIRCLE;

        public byte[] MOVEJ_P2P;

        public byte[] MoveJ_P2PB;

        public byte[] MOVEJ_HOME;

        public byte[] MOVE_SLEEP;

        public byte[] MOVE_SERVOJ;

        public byte[] PLAN_SLEEP;

        public byte[] TCP_JERK;

        public byte[] TCP_MAXACC;

        public byte[] JOINT_JERK;

        public byte[] JOINT_MAXACC;

        public byte[] TCP_OFFSET;

        public byte[] LOAD_PARAM;

        public byte[] GRAVITY_DIR;

        public byte[] COLLIS_SENS;

        public byte[] TEACH_SENS;

        public byte[] TCP_POS_CURR;

        public byte[] JOINT_POS_CURR;

        public byte[] CAL_IK;

        public byte[] CAL_FK;

        public byte[] IS_JOINT_LIMIT;

        public byte[] IS_TCP_LIMIT;

        public byte[] UTRC_INT32_NOW;

        public byte[] UTRC_INT8_NOW;

        public byte[] UTRC_FP32_NOW;

        public byte[] UTRC_INT8N_NOW;

        public byte[] UTRC_INT8_QUE;

        public byte[] UTRC_INT32_QUE;

        public byte[] UTRC_FP32_QUE;

        public byte[] UTRC_INT8N_QUE;

        public byte[] PASS_RS485_NOW;

        public byte[] PASS_RS485_QUE;

        public byte[] UTRC_U8FP32_NOW;

        public ARM_REG(byte axis)
        {
            AXIS = axis;
            Null = 0;

            // cmd的reg  读reg发送cmd的长度  读reg接收data的长度  写reg发送cmd的长度  写reg接收data的长度
            // Null 代表空值
            UUID = new byte[] { 0x01, 0, 17, Null, Null };
            SW_VERSION = new byte[] { 0x02, 0, 20, Null, Null };
            HW_VERSION = new byte[] { 0x03, 0, 20, Null, Null };
            UBOT_AXIS = new byte[] { 0x04, 0, 1, Null, Null };
            SYS_SHUTDOWN = new byte[] { 0x0B, Null, Null, 1, 0 };
            RESET_ERR = new byte[] { 0x0C, Null, Null, 1, 0 };
            SYS_REBOOT = new byte[] { 0x0D, Null, Null, 1, 0 };
            ERASE_PARM = new byte[] { 0x0E, Null, Null, 1, 0 };
            SAVED_PARM = new byte[] { 0x0F, Null, Null, 1, 0 };

            MOTION_MDOE = new byte[] { 0x20, 0, 1, 1, 0 };
            MOTION_ENABLE = new byte[] { 0x21, 0, 4, 2, 0 };
            BRAKE_ENABLE = new byte[] { 0x22, 0, 4, 2, 0 };
            ERROR_CODE = new byte[] { 0x23, 0, 2, Null, Null };
            SERVO_MSG = new byte[] { 0x24, 0, (byte)(AXIS * 2), Null, Null };
            MOTION_STATUS = new byte[] { 0x25, 0, 1, 1, 0 };
            CMD_NUM = new byte[] { 0x26, 0, 4, 4, 0 };

            MOVET_LINE = new byte[] { 0x30, Null, Null, 36, 4 };
            MOVET_LINEB = new byte[] { 0x31, Null, Null, 40, 4 };
            MOVET_CIRCLE = new byte[] { 0x32, Null, Null, 64, 4 };
            MOVET_P2P = new byte[] { 0x33, Null, Null, Null, Null };
            MOVET_P2PB = new byte[] { 0x34, Null, Null, Null, Null };
            MOVEJ_LINE = new byte[] { 0x35, Null, Null, Null, Null };
            MOVEJ_LINEB = new byte[] { 0x36, Null, Null, Null, Null };
            MOVEJ_CIRCLE = new byte[] { 0x37, Null, Null, Null, Null };
            MOVEJ_P2P = new byte[] { 0x38, Null, Null, (byte)((AXIS + 3) * 4), 4 };
            MoveJ_P2PB = new byte[] { 0x39, Null, Null, Null, Null };
            MOVEJ_HOME = new byte[] { 0x3A, Null, Null, 12, 4 };
            MOVE_SLEEP = new byte[] { 0x3B, Null, Null, 4, 4 };
            MOVE_SERVOJ = new byte[] { 0x3C, Null, Null, (byte)((AXIS + 3) * 4), 4 };
            PLAN_SLEEP = new byte[] { 0x3F, Null, Null, 4, 4 };

            TCP_JERK = new byte[] { 0x40, 0, 4, 4, 4 };
            TCP_MAXACC = new byte[] { 0x41, 0, 4, 4, 4 };
            JOINT_JERK = new byte[] { 0x42, 0, 4, 4, 4 };
            JOINT_MAXACC = new byte[] { 0x43, 0, 4, 4, 4 };
            TCP_OFFSET = new byte[] { 0x44, 0, 24, 24, 0 };
            LOAD_PARAM = new byte[] { 0x45, 0, 16, 16, 0 };
            GRAVITY_DIR = new byte[] { 0x46, 0, 12, 12, 0 };
            COLLIS_SENS = new byte[] { 0x47, 0, 1, 1, 0 };
            TEACH_SENS = new byte[] { 0x48, 0, 1, 1, 0 };

            TCP_POS_CURR = new byte[] { 0x50, 0, 24, Null, Null };
            JOINT_POS_CURR = new byte[] { 0x51, 0, (byte)(AXIS * 4), Null, Null };
            CAL_IK = new byte[] { 0x52, 24, (byte)(AXIS * 4), Null, Null };
            CAL_FK = new byte[] { 0x53, (byte)(AXIS * 4), 24, Null, Null };
            IS_JOINT_LIMIT = new byte[] { 0x54, (byte)(AXIS * 4), 1, Null, Null };
            IS_TCP_LIMIT = new byte[] { 0x55, 24, 1, Null, Null };

            //# [line id reg] [ret value] [line id reg value] [ret]
            UTRC_INT8_NOW = new byte[] { 0x60, 3, 2, 4, 1 };
            UTRC_INT32_NOW = new byte[] { 0x61, 3, 8, 7, 1 };
            UTRC_FP32_NOW = new byte[] { 0x62, 3, 8, 7, 1 };
            UTRC_INT8N_NOW = new byte[] { 0x63, 4, 0x55, 0x55, 1 };

            //# [line id reg] [ret value] [line id reg value] [ret]
            UTRC_INT8_QUE = new byte[] { 0x64, Null, Null, 4, 0 };
            UTRC_INT32_QUE = new byte[] { 0x65, Null, Null, 7, 0 };
            UTRC_FP32_QUE = new byte[] { 0x66, Null, Null, 7, 0 };
            UTRC_INT8N_QUE = new byte[] { 0x67, Null, Null, 0x55, 0 };

            PASS_RS485_NOW = new byte[] { 0x68, Null, Null, 0x55, 0x55 };
            PASS_RS485_QUE = new byte[] { 0x69, Null, Null, 0x55, 0 };

            // # [line id reg num] [ret value] [line id reg num value] [ret]
            UTRC_U8FP32_NOW = new byte[] { 0x6A, 4, 8, 8, 1 };
        }
    }
}
