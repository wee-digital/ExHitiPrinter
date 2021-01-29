Attribute VB_Name = "PavoApi"

' Copyright (C) 2008-2014 HiTi Digital, Inc.  All Rights Reserved.
'
' Revision: 1.7.2.930
' Date: 2014.05.06

'Messages sent by printer driver
Public Const WM_PAVO_PRINTER = &H5555

'possible WPARAM values for WM_PAVO_PRINTER
Public Const MSG_JOB_BEGIN = 1                              'Driver begin to process job
Public Const MSG_PRINT_ONE_PAGE = 3                         'Driver process one page
Public Const MSG_PRINT_ONE_COPY = 4                         'Driver send one copy of one page data to printer
Public Const MSG_JOB_END = 6                                'Driver process job end
Public Const MSG_DEVICE_STATUS = 7                          'Status of printer
Public Const MSG_JOB_CANCELED = 12                          'Driver cancel the job
Public Const MSG_JOB_PRINTED = 24                           'Printer print out the job completely

'HiTi defined Device Status Code used for PAVO_CheckPrinterStatus
Public Const PAVO_DS_BUSY = &H80000                         'Printer is busy
Public Const PAVO_DS_OFFLINE = &H80                         'Printer is not connected
Public Const PAVO_DS_PRINTING = &H2                         'Printer is printing
Public Const PAVO_DS_PROCESSING_DATA = &H5                  'Driver is processing print data
Public Const PAVO_DS_SENDING_DATA = &H6                     'Sending data to printer
Public Const PAVO_DS_CARD_MISMATCH = &H100FE                'Card mismatch
Public Const PAVO_DS_CMD_SEQ_ERROR = &H301FE                'Command sequence error
Public Const PAVO_DS_SRAM_ERROR = &H30001                   'SRAM error
Public Const PAVO_DS_SDRAM_ERROR = &H30101                  'SDRAM error
Public Const PAVO_DS_ADC_ERROR = &H30201                    'ADC error
Public Const PAVO_DS_NVRAM_ERROR = &H30301                  'NVRAM R/W error
Public Const PAVO_DS_SDRAM_CHECKSUM_ERROR = &H30302         'Check sum error - SDRAM
Public Const PAVO_DS_FW_WRITE_ERROR = &H30701               'Firmware write error
Public Const PAVO_DS_COVER_OPEN = &H50001                   'Cover or door open or Ajar
Public Const PAVO_DS_COVER_OPEN_SLAVE = &H50201             'Cover or door open or Ajar in slave printer
Public Const PAVO_DS_REJECT_BOX_MISSING = &H50301           'Rejected box missing
Public Const PAVO_DS_REJECT_BOX_FULL = &H50401              'Rejected box full
Public Const PAVO_DS_CARD_OUT = &H8000                      'Card out or feeding error
Public Const PAVO_DS_CARD_LOW = &H8001                      'Card low
Public Const PAVO_DS_RIBBON_MISSING = &H80004               'Ribbon missing
Public Const PAVO_DS_OUT_OF_RIBBON = &H80103                'Out of ribbon
Public Const PAVO_DS_RIBBON_IC_RW_ERROR = &H804FE           'Ribbon IC R/W error
Public Const PAVO_DS_UNSUPPORT_RIBBON = &H806FE             'Unsupported ribbon
Public Const PAVO_DS_UNKNOWN_RIBBON = &H808FE               'Unknown ribbon
Public Const PAVO_DS_RIBBON_MISSING_SLAVE = &H80204         'Ribbon missing in slave printer
Public Const PAVO_DS_OUT_OF_RIBBON_SLAVE = &H80303          'Out of ribbon in slave printer
Public Const PAVO_DS_RIBBON_IC_RW_ERROR_SLAVE = &H805FE     'Ribbon IC R/W error in slave printer
Public Const PAVO_DS_UNSUPPORT_RIBBON_SLAVE = &H807FE       'Unsupported ribbon in slave printer
Public Const PAVO_DS_UNKNOWN_RIBBON_SLAVE = &H809FE         'Unknown ribbon in slave printer
Public Const PAVO_DS_CARD_JAM1 = &H30000                    'Card jam in card path
Public Const PAVO_DS_CARD_JAM2 = &H40000                    'Card jam in flipper
Public Const PAVO_DS_CARD_JAM3 = &H50000                    'Card jam in eject box
Public Const PAVO_DS_CARD_JAM4 = &H30100                    'Card jam in card path of slave printer
Public Const PAVO_DS_CARD_JAM5 = &H40100                    'Card jam in flipper of slave printer
Public Const PAVO_DS_CARD_JAM6 = &H50100                    'Card jam in eject box of slave printer
Public Const PAVO_DS_CARD_JAM7 = &H60100                    'Card jam between master and slave
Public Const PAVO_DS_CARD_JAM8 = &H70100                    'Card jam in end of master
Public Const PAVO_DS_WRITE_FAIL = &H1F                      'WriteFile function error
Public Const PAVO_DS_READ_FAIL = &H2F                       'ReadFile function error
Public Const PAVO_DS_CARD_THICK_WRONG = &H8010              'Card thickness selector is not placed in the right position
Public Const PAVO_DS_NO_FLIPPER_MODULE = &H1100000D           'The flipper module is not attached

'following errors are used for CS-200e
Public Const PAVO_DS_0100_COVER_OPEN = &H100                '0100 Cover open
Public Const PAVO_DS_0101_FLIPPER_COVER_OPEN = &H101        '0101 Flipper cover open
Public Const PAVO_DS_0200_IC_MISSING = &H200                '0200 IC chip missing
Public Const PAVO_DS_0201_RIBBON_MISSING = &H201            '0201 Ribbon missing
Public Const PAVO_DS_0202_RIBON_MISMATCH = &H202            '0202 Ribbon mismatch
Public Const PAVO_DS_0203_RIBBON_TYPE_ERROR = &H203         '0203 Ribbon type error
Public Const PAVO_DS_0300_RIBBON_SEARCH_FAIL = &H300        '0300 Ribbon search fail
Public Const PAVO_DS_0301_RIBBON_OUT = &H301                '0301 Ribbon out
Public Const PAVO_DS_0302_PRINT_FAIL = &H302                '0302 Print fail
Public Const PAVO_DS_0303_PRINT_FAIL = &H303                '0303 Print fail
Public Const PAVO_DS_0304_RIBBON_OUT = &H304                '0304 Ribbon out
Public Const PAVO_DS_0400_CARD_OUT = &H400                  '0400 Card out
Public Const PAVO_DS_0500_CARD_JAM = &H500                  '0500 Card jam
Public Const PAVO_DS_0501_CARD_JAM = &H501                  '0501 Card jam
Public Const PAVO_DS_0502_CARD_JAM = &H502                  '0502 Card jam
Public Const PAVO_DS_0503_CARD_JAM = &H503                  '0503 Card jam
Public Const PAVO_DS_0504_CARD_JAM = &H504                  '0504 Card jam
Public Const PAVO_DS_0505_CARD_JAM = &H505                  '0505 Card jam
Public Const PAVO_DS_0506_CARD_JAM = &H506                  '0506 Card jam
Public Const PAVO_DS_0507_CARD_JAM = &H507                  '0507 Card jam
Public Const PAVO_DS_0508_CARD_JAM = &H508                  '0508 Card jam
Public Const PAVO_DS_0600_CARD_MISMATCH = &H600             '0600 Card mismatch
Public Const PAVO_DS_0700_CAM_ERROR = &H700                 '0700 Cam error
Public Const PAVO_DS_0800_FLIPPER_ERROR = &H800             '0800 Flipper error
Public Const PAVO_DS_0801_FLIPPER_ERROR = &H801             '0801 Flipper error
Public Const PAVO_DS_0802_FLIPPER_ERROR = &H802             '0802 Flipper error
Public Const PAVO_DS_0803_FLIPPER_ERROR = &H803             '0803 Flipper error
Public Const PAVO_DS_0900_NVRAM_ERROR = &H900               '0900 NVRAM error
Public Const PAVO_DS_1000_RIBBON_ERROR = &H1000             '1000 Ribbon error
Public Const PAVO_DS_1100_RBN_TAKE_CALIB_FAIL = &H1100      '1100 RBN Take Calibration Failed
Public Const PAVO_DS_1101_RBN_SUPPLY_CALIB_FAIL = &H1101    '1101 RBN Supply Calibration Failed
Public Const PAVO_DS_1200_ADC_ERROR = &H1200                '1200 ADC error
Public Const PAVO_DS_1300_FW_ERROR = &H1300                 '1300 FW error
Public Const PAVO_DS_1301_FW_ERROR = &H1301                 '1301 FW error
Public Const PAVO_DS_1400_POWER_SUPPLY_ERROR = &H1400       '1400 Power supply error
Public Const PAVO_DS_LOCKED = &H7540                        'Printer is locked

Public Const SCARD_E_NO_SMARTCARD = &H8010000C                'The operation requires a Smart Card, but no Smart Card is currently in the device.


'possible return value of PAVO_ReadMagTrackData() and PAVO_WriteMagTrackData()
Public Const ERROR_MAGCARD_CONNECT_FAIL = 1850              'Cannot connect COM port of magnetic module
Public Const ERROR_MAGCARD_READ_FAIL = 1851                 'Read track data fail
Public Const ERROR_MAGCARD_WRITE_FAIL = 1852                'Write track data fail
Public Const ERROR_MAGCARD_NO_TRACK_SELECTED = 1853         'No track is specified to be read/write
Public Const ERROR_MAGCARD_EMPTY_TRACK_DATA = 1855          'One of the track data is empty
Public Const ERROR_MAGCARD_NO_MODULE = 1856                 'The magnetic stripe encoder module is not attached

'Possible value of dwInfoType for PAVO_GetDeviceInfo()
Public Const PAVO_DEVINFO_MFG_SERIAL = 1                    'Manufacture serial number
Public Const PAVO_DEVINFO_MODEL_NAME = 2                    'Printer model name
Public Const PAVO_DEVINFO_FIRMWARE_VERSION = 3              'Printer firmware version
Public Const PAVO_DEVINFO_RIBBON_INFO = 4                   'Current ribbon information
Public Const PAVO_DEVINFO_PRINT_COUNT = 5                   'Number of printed cards
Public Const PAVO_DEVINFO_CARD_POSITION = 6                 'Current card position

'Possible value of dwCommand for PAVO_DoCommand()
Public Const PAVO_COMMAND_RESET_PRINTER = 100               'Reset Printer to initial state
Public Const PAVO_COMMAND_CLEAN_CARD_PATH = 105             'Clean card path
Public Const PAVO_COMMAND_RESET_PRINT_COUNT = 106           'Reset print count
Public Const PAVO_COMMAND_FLIP_CARD = 202                   'Flip card by flipper
Public Const PAVO_COMMAND_AUTO_FEED_FROM_FLIPPER_ON = 203   '!< Auto card feed from flipper ON
Public Const PAVO_COMMAND_AUTO_FEED_FROM_FLIPPER_OFF = 204  '!< Auto card feed from flipper OFF
Public Const PAVO_COMMAND_RESET_PRINTER_CLEAR_JAM = 205     '!< Reset Printer to initial state and clear card jam alert
Public Const PAVO_COMMAND_TIGHTEN_RIBBON = 206              '!< Tighten the ribbon with take motor
Public Const PAVO_COMMAND_ERASE_CARD_BY_DRIVER = 207        '!< Erase card for CS-230e
Public Const PAVO_COMMAND_DETECT_CARD_EMPTY = 209           '!< Detect card empty, 2018.04.12



'HiTi defined ribbon type used for setting PAVO_JOB_PROPERTY.byRibbonType and PAVO_GetDeviceInfo()
Public Const PAVO_RIBBON_TYPE_YMCKO = 0
Public Const PAVO_RIBBON_TYPE_K = 1
Public Const PAVO_RIBBON_TYPE_KO = 3
Public Const PAVO_RIBBON_TYPE_YMCKOK = 4
Public Const PAVO_RIBBON_TYPE_HALF_YMCKO = 5
Public Const PAVO_RIBBON_TYPE_YMCKFO = 12

'HiTi defined card type used for setting PAVO_JOB_PROPERTY.dwCardType
Public Const PAVO_CARD_TYPE_BLANK_CARD = 0                  'Blank Card
Public Const PAVO_CARD_TYPE_SMART_CHIP_6PIN = 1             'Smart Chip Card 6-pin
Public Const PAVO_CARD_TYPE_SMART_CHIP_8PIN = 2             'Smart Chip Card 8-pin
Public Const PAVO_CARD_TYPE_MAG_STRIP = 3                   'Magnetic Stripe Card
Public Const PAVO_CARD_TYPE_CHIP_MAG_STRIP = 4              'Chip/Magnetic Card
Public Const PAVO_CARD_TYPE_ADHESIVE_CARD = 5               'Adhesive Card

'used for setting PAVO_JOB_PROPERTY.dwFlags
Public Const PAVO_FLAG_NOT_SHOW_ERROR_MSG_DLG = &H1          'Not show error message dialog
Public Const PAVO_FLAG_WAIT_MSG_DONE = &H2                   'Make driver to wait until AP process the message ok
Public Const PAVO_FLAG_NOT_SHOW_CLEAN_MSG = &H100            'Not popup clean message
Public Const PAVO_FLAG_WATCH_JOB_PRINTED = &H400             'Indicate driver to notify AP when print card completely
Public Const PAVO_FLAG_MOVE_CARD_TO_STANDBY_AFTER_PRINTED = &H10000 'Move card to standby position after printing finished


'used for setting PAVO_JOB_PROPERTY.dwDataFlag
Public Const PAVO_DATAFLAG_RESIN_FRONT = &H2
Public Const PAVO_DATAFLAG_RESIN_BACK = &H10

'used for dwType of PAVO_SetExtraDataToHDC
Public Const PAVO_DATA_RESIN_FRONT = 2
Public Const PAVO_DATA_RESIN_BACK = 5

'used for setting PAVO_JOB_PROPERTY.byDuplex
Public Const PAVO_DUPLEX_PRINT_FRONT_SIDE = 1
Public Const PAVO_DUPLEX_PRINT_BACK_SIDE = 2


'Parameter definition for PAVO_MoveCard()
Public Const MOVE_CARD_TO_IC_ENCODER = 1                    'Move Card to Contact Encoder Station
Public Const MOVE_CARD_TO_RFID_ENCODER = 3                  'Move Card to Contactless Encoder Station
Public Const MOVE_CARD_TO_REJECT_BOX = 4                    'Move Card to Reject Box
Public Const MOVE_CARD_TO_HOPPER = 5                        'Move Card to Output Hopper
Public Const MOVE_CARD_TO_FLIPPER = 6                       'Move Card to Flipper
Public Const MOVE_CARD_TO_PRINT_FROM_FLIPPER = 7            'Move Card to Print Position from Flipper
Public Const MOVE_CARD_TO_STANDBY_POSITION = 10             'Move Card to Standby Position


'PAVO_JOB_PROPERTY.dwFieldFlag
Public Const FF_CARD_TYPE = &H1
Public Const FF_FLAGS = &H2
Public Const FF_DATA_FLAG = &H4
Public Const FF_PARENT_HWND = &H8
Public Const FF_ORIENTATION = &H20
Public Const FF_COPIES = &H40
Public Const FF_CUSTOM_INDEX = &H80
Public Const FF_DUPLEX = &H100
Public Const FF_RIBBON_TYPE = &H200
Public Const FF_PRINT_COLOR = &H400
Public Const FF_DITHER_K = &H800
Public Const FF_LAMIN = &H1000
Public Const FF_LAMIN_TYPE = &H2000
Public Const FF_ROTATE180 = &H4000
Public Const FF_CARD_THICK = &H10000
Public Const FF_TRANSPARENT_AND_FLIP = &H20000
Public Const FF_ALL_FIELDS = &HFFFFFFFF

'used for PAVO_ApplyJobSetting
Public Type PAVO_JOB_PROPERTY

    dwSize              As Long             '=80
    dwCardType          As Long
    dwFlags             As Long
    dwDataFlag          As Long

    hParentWnd          As Long
    hReserved1          As Long

    pReserved1          As String
    pReserved2          As String
    pReserved3          As String
    pReserved4          As String

    shOrientation       As Integer          '1=Portrait,2=Landscape
    shCopies            As Integer
    shReserved1         As Integer
    shReserved2         As Integer

    dwCustomIndex       As Long
    dwFieldFlag         As Long
    dwReserved3         As Long
    dwReserved4         As Long

    wReserved1          As Integer
    bReserved1          As Byte
    byPrintAs230e       As Byte             '2017.05.11 changed by Bill for CS290e also support CS230e no ribbon rewrite
    byTransparentCard   As Byte             '0=use white card;1=use transparent card. 2013.03.06
    byFlip              As Byte             '0=no flip;0x01=flip front side;0x02=flip back side;0x03=flip both side. 2013.03.06
    byReserved1         As Byte
    byCardThick         As Byte             '0=0.3mm;1=0.5mm;2=0.8mm;3=1.0mm

    byDuplex            As Byte             'Print side: 0x01=front side, 0x02=back side
    byRibbonType        As Byte             '0=YMCKO;1=K;2=YMCO;3=KO;4=YMCKOK;5=1/2YMCKO;6=Gold;7=Silver;8=White
    byPrintColor        As Byte             'Print YMCO: 0x01=front side, 0x02=back side
    byDitherK           As Byte             'Print K with dither: 0x01=front side, 0x02=back side
    byLamin             As Byte             'Print lamination: 0x01=front side, 0x02=back side
    byReserved6         As Byte
    byLaminType         As Byte             'Lamination ribbon type, 0x00=HiTi Standard Overlay, 0x01=HiTi Standard Patch
    byRotate180         As Byte             'Rotate 180: 0x00=Not rotate, 0x01=front side, 0x02=back side, 0x03=both sides

End Type


'used for PAVO_PrintOneCard()
Public Type HITI_CARD_PRINT_PARAMETER

    dwSize              As Long
    dwReserve1          As Long
    dwReserve2          As Long
    dwFlags             As Long

    byOrientation       As Byte         '1=portrait;2=landscape
    byCardThickness     As Byte         '0=0.3mm;1=0.5mm;2=0.8mm;3=1.0mm
        byTransparentCard   As Byte         '0=white card, 1=transparent card
    byReserve4          As Byte
    byReserve5          As Byte
    byReserve6          As Byte
    byReserve7          As Byte
    byReserve8          As Byte

    lpFrontBGR          As Long         '24-bits, BGRBGR...
    lpFrontK            As Long         '8-bits, each pixel with value 0x00 will be transfer to card by ribbon K
    lpFrontO            As Long         '8-bits, each pixel with value 0xFF will be transfer to card by ribbon O

    lpBackBGR           As Long         '24-bits, BGRBGR...
    lpBackK             As Long         '8-bits, each pixel with value 0x00 will be transfer to card by ribbon K
    lpBackO             As Long         '8-bits, each pixel with value 0xFF will be transfer to card by ribbon O

    lpReserve1          As Long
    lpReserve2          As Long

End Type

'used for PAVO_PrintOneCard()
Public Type HITI_HEATING_ENERGY

    'Heating Energy, value range of following fields are -127 ~ 127
    'same as driver UI Heating Energy tab
    'Density Adjustment ------------------------
    'Front side
    chFrontDenYMC       As Byte
    chFrontDenK         As Byte
    chFrontDenO         As Byte
    chFrontDenResinK    As Byte

    'Back side
    chBackDenYMC        As Byte
    chBackDenK          As Byte
    chBackDenO          As Byte
    chBackDenResinK     As Byte

    'Sensitivity Adjustment --------------------
    'Front side
    chFrontSenYMC       As Byte
    chFrontSenK         As Byte
    chFrontSenO         As Byte
    chFrontSenResinK    As Byte

    'Back side
    chBackSenYMC        As Byte
    chBackSenK          As Byte
    chBackSenO          As Byte
    chBackSenResinK     As Byte

End Type


'used for PAVO_ReadMagTrackData() and PAVO_WriteMagTrackData()
Public Type MAG_TRACK_DATA2

    szTrack1            As String * 256     '78 characters max, allow characters are between {}, { !"#$&'()*+,-./0123456789:;<=>@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_}. Start Sentinel is '%', End Sentinel is '?', both will be auto added by encoder.
    szTrack2            As String * 256     '39 characters max, allow characters are between {}, {0123456789:<=>}. Start Sentinel is ';', End Sentinel is '?', both will be auto added by encoder.
    szTrack3            As String * 256     '105 characters max, allow characters are between {}, {0123456789:<=>}. Start Sentinel is ';', End Sentinel is '?', both will be auto added by encoder.

    byTrackFlag         As Byte             '0x01=track1, 0x02=track2, 0x04=track3
    byEncodeMode        As Byte             'not used
    byCoercivity        As Byte             '0=Lo-Co, 1=Hi-Co
    byT2BPI             As Byte             '75 or 210, if not set it, will use 75

    byRawLenT1          As Byte
    byRawLenT2          As Byte
    byRawLenT3          As Byte

    byReserve(0 To 24)  As Byte

End Type


Public Declare Function PAVO_ApplyJobSetting Lib "PavoApi.DLL" (ByVal szPrinter As String, ByVal hDC As Long, ByVal lpInDevMode As Long, ByRef lpInJobProp As PAVO_JOB_PROPERTY) As Long
Public Declare Function PAVO_CheckPrinterStatus Lib "PavoApi.DLL" (ByVal szPrinter As String, ByRef dwStatus As Long) As Long
Public Declare Function PAVO_DoCommand Lib "PavoApi.DLL" (ByVal szPrinter As String, ByVal dwCommand As Long) As Long
Public Declare Function PAVO_FindComPort Lib "PavoApi.DLL" (ByVal szPrinter As String, ByRef dwMagPort As Long, ByRef dwRFPort As Long) As Long
Public Declare Function PAVO_FindSCardReader Lib "PavoApi.DLL" (ByVal szPrinter As String, ByVal szReaderName As String) As Long
Public Declare Function PAVO_GetDeviceInfo Lib "PavoApi.DLL" (ByVal szPrinter As String, ByVal dwInfoType As Long, lpInfoData As Any, ByRef lpdwDataLen As Long) As Long
Public Declare Function PAVO_MoveCard Lib "PavoApi.DLL" (ByVal szPrinter As String, ByVal dwPosition As Long) As Long
Public Declare Function PAVO_PrintOneCard Lib "PavoApi.DLL" (ByVal szPrinter As String, ByRef lpJobPara As HITI_CARD_PRINT_PARAMETER, ByRef lpHeatEnergy As HITI_HEATING_ENERGY, ByVal lpReserved As Byte) As Long
Public Declare Function PAVO_ReadMagTrackData Lib "PavoApi.DLL" (ByVal szPrinter As String, ByVal dwCOM As Long, ByRef lpTrackData As MAG_TRACK_DATA2) As Long
Public Declare Function PAVO_SetExtraDataToHDC Lib "PavoApi.DLL" (ByVal hDC As Long, ByVal dwType As Long, ByVal x As Long, ByVal y As Long, ByRef lpBmp As BITMAP) As Long
Public Declare Function PAVO_SetPassword Lib "PavoApi.DLL" (ByVal szPrinter As String, ByVal szCurrentPasswd As String, ByVal szNewPasswd As String) As Long
Public Declare Function PAVO_SetSecurityMode Lib "PavoApi.DLL" (ByVal szPrinter As String, ByVal szCurrentPasswd As String, ByVal nSecurityMode As Long) As Long
Public Declare Function PAVO_SetStandbyParameters Lib "PavoApi.DLL" (ByVal szPrinter As String, byStandbyPos As Byte, byStandbyTime As Byte) As Long
Public Declare Function PAVO_WriteMagTrackData Lib "PavoApi.DLL" (ByVal szPrinter As String, ByVal dwCOM As Long, ByRef lpTrackData As MAG_TRACK_DATA2) As Long
