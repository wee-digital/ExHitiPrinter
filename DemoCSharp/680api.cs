using System;
using System.Runtime.InteropServices;

namespace CS2xxRFID
{
	public class E680API
	{
		#region Export Function Prototypes
		[DllImport("680api.dll")]
		public static extern int E680_Open_ComPort(int port);

		[DllImport("680api.dll")]
		public static extern int E680_Close_ComPort();

		[DllImport("680api.dll")]
		public static extern int E680_get_version(byte[] version);

		[DllImport("680api.dll")]
		public static extern int E680_Request_CardSN(byte[] serial);

		[DllImport("680api.dll")]
		public static extern int E680_loadkey(byte keyid, byte[] keyvalue);

		[DllImport("680api.dll")]
		public static extern int E680_block_read(byte block, byte keytype, byte keyid, byte[] data);

		[DllImport("680api.dll")]
        public static extern int E680_block_write(byte block, byte keytype, byte keyid, byte[] data);

		[DllImport("680api.dll")]
        public static extern int E680_sector_read(byte block, byte keytype, byte keyid, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_initvalue(byte block, byte keytype, byte keyid, int value);

		[DllImport("680api.dll")]
		public static extern int E680_readvalue(byte block, byte keytype, byte keyid, ref int value);

		[DllImport("680api.dll")]
		public static extern int E680_increment(byte block, byte keytype, byte keyid, uint value);

		[DllImport("680api.dll")]
        public static extern int E680_decrement(byte block, byte keytype, byte keyid, uint value);

		[DllImport("680api.dll")]
		public static extern int E680_ledset(byte state);

		[DllImport("680api.dll")]
		public static extern int E680_buzzerset(byte time);

		[DllImport("680api.dll")]
		public static extern int E680_eeprom_read(int startadd, byte length, byte[] data);

		[DllImport("680api.dll")]
        public static extern int E680_eeprom_write(int startadd, byte length, byte[] data);

		[DllImport("680api.dll")]
        public static extern int E680_UL_read(byte page, byte[] data);

		[DllImport("680api.dll")]
        public static extern int E680_UL_write(byte page, byte[] data);

		[DllImport("680api.dll")]
        public static extern int E680_reset_SAM(ref int infolen, byte[] info);

		[DllImport("680api.dll")]
        public static extern int E680_send_APDU_to_SAM(int apdulen, byte[] apdu, ref int outlen, byte[] response);

		[DllImport("680api.dll")]
		public static extern int E680_loadkey_by_string(byte keyid, byte[] keyvalue);

		[DllImport("680api.dll")]
		public static extern int E680_block_read_by_string(byte block, byte keytype, byte keyid, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_block_write_by_string(byte block, byte keytype, byte keyid, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_sector_read_by_string(byte block, byte keytype, byte keyid, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_eeprom_read_by_string(int startadd, byte length, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_eeprom_write_by_string(int startadd, byte length, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_UL_read_by_string(byte page, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_UL_write_by_string(byte page, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_reset_SAM_by_string(ref int infolen, byte[] info);

		[DllImport("680api.dll")]
		public static extern int E680_send_APDU_to_SAM_by_string(int apdulen, byte[] apdu, ref int outlen, byte[] response);

		[DllImport("680api.dll")]
		public static extern int E680_Set_Working_Mode(byte antenna, byte autoq);

		[DllImport("680api.dll")]
		public static extern int E680_Set_Readcard_Mode(byte cardtype);

		[DllImport("680api.dll")]
		public static extern int E680_15693_Inventory(byte[] serial);

		[DllImport("680api.dll")]
		public static extern int E680_15693_stay_quiet();

		[DllImport("680api.dll")]
		public static extern int E680_15693_get_tag_information(byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_15693_reset_quiet(byte[] uid);

		[DllImport("680api.dll")]
		public static extern int E680_15693_read_blocks(byte start, byte blocks, byte[] data);

		[DllImport("680api.dll")]
        public static extern int E680_15693_write_blocks(byte start, byte blocks, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_15693_lock_block(byte block);

		[DllImport("680api.dll")]
		public static extern int E680_15693_write_AFI(byte afi);

		[DllImport("680api.dll")]
		public static extern int E680_15693_lock_AFI();

		[DllImport("680api.dll")]
		public static extern int E680_15693_write_DSFID(byte dsfid);

		[DllImport("680api.dll")]
		public static extern int E680_15693_lock_DSFID();

		[DllImport("680api.dll")]
        public static extern int E680_15693_get_blocks_security(byte start, byte blocks, byte[] data);

		[DllImport("680api.dll")]
        public static extern int E680_15693_read_blocks_by_string(byte start, byte blocks, byte[] data);

		[DllImport("680api.dll")]
        public static extern int E680_15693_write_blocks_by_string(byte start, byte blocks, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_14443B_card_reset(byte[] serial);

		[DllImport("680api.dll")]
		public static extern int E680_14443B_card_halt(byte[] pupi);

		[DllImport("680api.dll")]
		public static extern int E680_SR_initiate(ref byte chipid);

		[DllImport("680api.dll")]
        public static extern int E680_SR_pcall16(ref byte chipid);

		[DllImport("680api.dll")]
        public static extern int E680_SR_slot_marker(byte slot, ref byte chipid);

		[DllImport("680api.dll")]
		public static extern int E680_SR_select(byte chipid);

		[DllImport("680api.dll")]
		public static extern int E680_SR_completion();

		[DllImport("680api.dll")]
		public static extern int E680_SR_reset_to_inventory();

		[DllImport("680api.dll")]
		public static extern int E680_SR_authenticate(byte[] rnd, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_SR_get_uid(byte[] uid);

		[DllImport("680api.dll")]
		public static extern int E680_SR_read_block(byte addr, byte[] data);

		[DllImport("680api.dll")]
		public static extern int E680_SR_write_block(byte addr, byte[] data);
		#endregion
	}
}