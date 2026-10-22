using Dm;
using Google.Protobuf.Compiler;
using HCSRunTimeLibrary;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.GM;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml;
using ZLUniSource;
using static FreeSql.Internal.GlobalFilter;
using static Mysqlx.Expect.Open.Types.Condition.Types;
namespace FlowRunCode
{
    //-------------------------------流程----------------------------------
    public class F63e4f59e73224d8183b7aed9ef60b843 : IFlowRun
    {
        public string FlowId => "a55e0c2a-dda9-48cf-82e0-240ef2066a58";
        public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();
        public int AddedCount { get; private set; }

        public F63e4f59e73224d8183b7aed9ef60b843()
        {
        }
        public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
        {
            FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
            FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
            try
            {
                await f2169223d3c9f48c3aff6e7a87f2852df(context);
                feedBackInfo.ReturnMsg = context.ReturnMsg;
            } catch (Exception ex)
            {
                feedBackInfo.ReturnMsg = ex.Message;
                feedBackInfo.IsError = true;
            }
            feedBackInfo.Parameter = parameter;

            return feedBackInfo;
        }


        //主方法
        private async Task f2169223d3c9f48c3aff6e7a87f2852df(FlowContext context)
        {
            object ReadArg(string key)
            {
                try
                {
                    object bodyValue = null;
                    try
                    {
                        bodyValue = context.Body?[key];
                    } catch (Exception exBody)
                    {
                        throw new Exception("读取 context.Body 参数失败，参数名：" + key + "。详细：" + exBody.Message, exBody);
                    }

                    if (bodyValue != null) return bodyValue;

                    try
                    {
                        return context[key];
                    } catch (Exception exCtx)
                    {
                        throw new Exception("读取 context 参数失败，参数名：" + key + "。详细：" + exCtx.Message, exCtx);
                    }
                } catch (Exception ex)
                {
                    throw new Exception("读取入参失败，参数名：" + key + "。详细：" + ex.Message, ex);
                }
            }

            string ToStr(object value)
            {
                return value == null ? null : value.ToString();
            }

            WebCallInput _input = new WebCallInput();

            _input.url = ToStr(ReadArg("url"));
            _input.appId = ToStr(ReadArg("appId"));
            _input.appSecret = ToStr(ReadArg("appSecret"));
            _input.platformPublicKey = ToStr(ReadArg("platformPublicKey"));
            _input.clientSM2PublicKey = ToStr(ReadArg("clientSM2PublicKey"));
            _input.clientSM2PrivateKey = ToStr(ReadArg("clientSM2PrivateKey"));
            _input.serviceCode = ToStr(ReadArg("serviceCode"));
            _input.data = ToStr(ReadArg("data"));

            //调用接口
            string result = HipHttpPost(_input);

            //返回数据输出
            context.SetParamValue(@"data", result);
            context.ReturnMsg = context.GetParamValue("data");
        }

        //业务方法
        public string HipHttpPost(WebCallInput input)
        {
            //校验
            ValidateConfig(input);

            // 获取当前UTC时间的毫秒级时间戳
            string strTimestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            //1.组织业务数据            
            //界面json对象转为JObject
            string jsonStr = input.data?.ToString();
            JObject jObj = JObject.Parse(jsonStr);
            //组装body head+body
            string businessData = GetPostBodyString(input.serviceCode, jObj, strTimestamp);

            //2.生成随机16字节密钥
            byte[] sm4Key = SM4CryptoHelper.GenerateRandomKey();

            //3.对业务数据加密
            string encryptedData = SM4CryptoHelper.EncryptCBCToHex(businessData, sm4Key);   //SM4-CBC 加密

            //4.使用临时SM4密钥加密业务报文；再用平台SM2公钥加密该临时密钥。
            string encryptedKey = SM2CryptoHelper.EncryptBytes(sm4Key, input.platformPublicKey, SM2KeyEncoding.Base64);

            //5.组织消息头
            //加密认证 MD5
            string newAuthor = GetAuthor(input, strTimestamp);
            //组织消息头
            HipHttpHeader httpHeader = new HipHttpHeader();  //组织header消息头
            httpHeader.Authorization = newAuthor;
            httpHeader.AppId = input.appId;
            httpHeader.Timestamp = strTimestamp;
            httpHeader.Version = "1.0";
            httpHeader.EncType = "SM4";
            httpHeader.SignType = "SM2";
            httpHeader.EncryptedKey = encryptedKey;   //客户端公钥加密的临时 SM4 密钥
            httpHeader.SignData = "";

            //6.签名
            //构建消息头
            Dictionary<string, string> headDic = GetHeaderDic(input, newAuthor, strTimestamp, encryptedKey);
            //按接口规范拼装签名原文，最后使用客户端SM2私钥生成签名。
            string signPlainText = BuildSignPlainText(headDic, encryptedData, input.appSecret);
            httpHeader.SignData = SM2CryptoHelper.Sign(signPlainText, input.clientSM2PrivateKey);

            //7.发送消息
            string result = SM2KeyHelper.HttpPost(input.url, encryptedData, AuthType.Oauth, httpHeader, out Dictionary<string, string> responseHeaders, 10 * 60);

            //8.返回数据
            //字符串解密 
            string msg = "";
            result = DecryptResponseData(result, responseHeaders, input);
            //JObject jobjResult = JObject.Parse(result);
            //if (jobjResult["head"] == null || jobjResult["head"]["code"] == null)
            //{
            //    msg = "返回数据格式不正确！";
            //    throw new($"【区卫医共体】查询服务失败，错误信息：{msg}");
            //}
            //else
            //{
            //    if (jobjResult["head"]["code"].ToString() != "1")
            //    {
            //        msg = jobjResult["head"]["msg"]?.ToString();
            //        throw new($"【区卫医共体】查询服务失败，错误信息：{msg}");
            //    }
            //    else
            //    {
            //        msg = "同步区卫医共体数据成功！";
            //    }
            //}


            return result;
        }


        //--------------------------------私有方法----------------------------------
        #region 私有方法
        /// <summary>
        /// 校验接口调用所需的最小配置项。
        /// </summary>
        /// <param name="config">EMPI配置</param>
        static void ValidateConfig(WebCallInput config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            if (string.IsNullOrWhiteSpace(config.appId))
            {
                throw new Exception("基础配置缺少AppId。");
            }
            if (string.IsNullOrWhiteSpace(config.appSecret))
            {
                throw new Exception("基础配置缺少SecretKey。");
            }
            if (string.IsNullOrWhiteSpace(config.platformPublicKey))
            {
                throw new Exception("基础配置缺少平台公钥。");
            }
            if (string.IsNullOrWhiteSpace(config.clientSM2PublicKey))
            {
                throw new Exception("基础配置缺少服务端SM2公钥。");
            }
            if (string.IsNullOrWhiteSpace(config.clientSM2PrivateKey))
            {
                throw new Exception("基础配置缺少客户端SM2私钥。");
            }
        }

        /// <summary>
        /// 组织body head+业务data
        /// </summary>
        /// <returns></returns>
        static string GetPostBodyString(string serviceCode, JObject businessData, string timestamp)
        {
            HipRequestRoot bodyObj = new HipRequestRoot();
            HipHead headObj = new HipHead();

            headObj.id = Guid.NewGuid().ToString();
            headObj.request_timestamp = timestamp;  // DateTime.Now.ToString("yyyyMMddHHmmss");
            headObj.app_code = "zlbasedt";
            headObj.target_app_code = "HIP";
            headObj.service_code = serviceCode;

            bodyObj.head = headObj;
            bodyObj.body = businessData;

            string strBody = JsonConvert.SerializeObject(bodyObj);
            return strBody;
        }

        /// <summary>
        /// 认证加密 MD5
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="key"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        static string GetAuthor(WebCallInput input, string timestamp)
        {
            string newAuthor = $"SOFTUN|{input.appId}|{input.appSecret}|{timestamp}|SOFTUN";
            //MD5加密
            string encAuthor = SM2KeyHelper.ComputeMD5Hash(newAuthor);

            return encAuthor;
        }

        /// <summary>
        /// 构建消息头
        /// </summary>
        /// <param name="input"></param>
        /// <param name="author"></param>
        /// <param name="timestamp"></param>
        /// <param name="encryptedKey"></param>
        /// <returns></returns>
        static Dictionary<string, string> GetHeaderDic(WebCallInput input, string author, string timestamp, string encryptedKey)
        {

            Dictionary<string, string> headStr = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["Authorization"] = "Bearer " + author,
                ["Content-Type"] = "text/plain",
                ["appId"] = input.appId,
                ["timestamp"] = timestamp,
                ["version"] = "1.0",
                ["enc-type"] = "SM4",
                ["sign-type"] = "SM2",
                ["encrypted-key"] = encryptedKey,
                ["sign-data"] = string.Empty
            };

            return headStr;
        }

        /// <summary>
        /// 按接口文档约定构造待签名原文。
        /// 参与签名的Header不包含sign-data，并且必须按Header名称ASCII升序排列。
        /// </summary>
        /// <param name="headers">请求头集合</param>
        /// <param name="encryptedData">SM4加密后的业务数据</param>
        /// <param name="appSecret">平台分配的应用密钥</param>
        /// <returns></returns>
        static string BuildSignPlainText(Dictionary<string, string> headers, string encryptedData, string appSecret)
        {
            SortedDictionary<string, string> signHeaders = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                ["appId"] = headers["appId"],
                ["enc-type"] = headers["enc-type"],
                ["encrypted-key"] = headers["encrypted-key"],
                ["sign-type"] = headers["sign-type"],
                ["timestamp"] = headers["timestamp"],
                ["version"] = headers["version"]
            };

            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> item in signHeaders)
            {
                if (builder.Length > 0)
                {
                    builder.Append("&");
                }
                builder.Append(item.Key).Append("=").Append(item.Value);
            }

            builder.Append("&encrypted-data=").Append(encryptedData);
            builder.Append("&key=").Append(appSecret);
            return builder.ToString();
        }

        /// <summary>
        /// 解析EMPI响应数据，解密Body并返回JSON字符串。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signData">签名数据</param>
        /// <param name="privateKeyBase64"></param>
        /// <returns></returns>
        static string DecryptResponseData(string data, Dictionary<string, string> headers, WebCallInput config)
        {
            //1.用平台 SM2 公钥验签响应 sign-data（验签原文构造规则同请求）。
            string signPlainText = BuildSignPlainText(headers, data, config.appSecret);
            bool isValid = SM2CryptoHelper.Verify(signPlainText, headers["sign-data"], config.platformPublicKey);
            if (!isValid)
            {
                throw new Exception("【区卫医共体】个人信息查询服务返回数据验签失败");
            }

            //2.用客户端 SM2 私钥解密 encrypted-key 得到临时 SM4 密钥 K2
            byte[] bytSM4Key = SM2CryptoHelper.DecryptToBytes(headers["encrypted-key"], config.clientSM2PrivateKey, SM2KeyEncoding.Base64);

            //3.用 K2 解密 Body（十六进制→密文字节→SM4 CBC 解密）得到响应 JSON。
            string result = SM4CryptoHelper.DecryptCBCFromHex(data, bytSM4Key);
            return result;
        }

        #endregion


    }

    //--------------------------------公共方法----------------------------------
    #region 公共方法
    /// <summary>
    /// <summary>
    /// 认证类型
    /// </summary>
    public enum AuthType
    {
        None,
        Basic,
        Oauth
    }
    /// <summary>
    /// SM2签名/验签/加解密工具类
    /// </summary>
    public static class SM2CryptoHelper
    {
        #region 签名

        /// <summary>
        /// SM2签名（SM3withSM2）
        /// </summary>
        /// <param name="data">待签名数据</param>
        /// <param name="privateKey">私钥字符串</param>
        /// <param name="encoding">密钥编码格式</param>
        /// <returns>Base64编码的签名值</returns>
        public static string Sign(string data, string privateKey, SM2KeyEncoding encoding = SM2KeyEncoding.Base64)
        {
            try
            {
                var keyParams = SM2KeyHelper.LoadPrivateKey(privateKey, encoding);
                return Sign(data, keyParams);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2签名失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM2签名（使用密钥参数）
        /// </summary>
        public static string Sign(string data, ECPrivateKeyParameters privateKey)
        {
            try
            {
                var signer = new SM2Signer();
                signer.Init(true, new ParametersWithRandom(privateKey, new SecureRandom()));
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                signer.BlockUpdate(dataBytes, 0, dataBytes.Length);
                byte[] signature = signer.GenerateSignature();
                return Convert.ToBase64String(signature);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2签名失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM2签名（Hex输出）
        /// </summary>
        public static string SignToHex(string data, string privateKey, SM2KeyEncoding encoding = SM2KeyEncoding.Base64)
        {
            byte[] signature = Convert.FromBase64String(Sign(data, privateKey, encoding));
            return SM2KeyHelper.BytesToHex(signature);
        }

        #endregion

        #region 验签

        /// <summary>
        /// SM2验签
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="signature">Base64编码的签名值</param>
        /// <param name="publicKey">公钥字符串</param>
        /// <param name="encoding">密钥编码格式</param>
        /// <returns>验签是否通过</returns>
        public static bool Verify(string data, string signature, string publicKey, SM2KeyEncoding encoding = SM2KeyEncoding.Base64)
        {
            try
            {
                var keyParams = SM2KeyHelper.LoadPublicKey(publicKey, encoding);
                return Verify(data, signature, keyParams);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2验签失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// SM2验签（使用密钥参数）
        /// </summary>
        public static bool Verify(string data, string signature, ECPublicKeyParameters publicKey)
        {
            try
            {
                var signer = new SM2Signer();
                signer.Init(false, publicKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                signer.BlockUpdate(dataBytes, 0, dataBytes.Length);
                byte[] signatureBytes = Convert.FromBase64String(signature);
                return signer.VerifySignature(signatureBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2验签失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// SM2验签（Hex签名输入）
        /// </summary>
        public static bool VerifyWithHexSignature(string data, string hexSignature, string publicKey, SM2KeyEncoding encoding = SM2KeyEncoding.Base64)
        {
            byte[] signatureBytes = SM2KeyHelper.HexToBytes(hexSignature);
            string base64Signature = Convert.ToBase64String(signatureBytes);
            return Verify(data, base64Signature, publicKey, encoding);
        }

        #endregion

        #region 加密

        /// <summary>
        /// SM2公钥加密
        /// </summary>
        /// <param name="data">明文数据</param>
        /// <param name="publicKey">公钥字符串</param>
        /// <param name="encoding">密钥编码格式</param>
        /// <returns>Base64编码的密文</returns>
        public static string Encrypt(string data, string publicKey, SM2KeyEncoding encoding = SM2KeyEncoding.Base64)
        {
            try
            {
                var keyParams = SM2KeyHelper.LoadPublicKey(publicKey, encoding);
                return Encrypt(data, keyParams);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2加密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM2加密（使用密钥参数）
        /// </summary>
        public static string Encrypt(string data, ECPublicKeyParameters publicKey)
        {
            try
            {
                var engine = new SM2Engine();
                engine.Init(true, new ParametersWithRandom(publicKey, new SecureRandom()));
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] cipherBytes = engine.ProcessBlock(dataBytes, 0, dataBytes.Length);
                // BC默认输出c1||c2||c3，转为国标c1||c3||c2
                cipherBytes = ChangeC1C2C3ToC1C3C2(cipherBytes);
                return Convert.ToBase64String(cipherBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2加密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM2加密字节数组
        /// </summary>
        public static string EncryptBytes(byte[] data, string publicKey, SM2KeyEncoding encoding = SM2KeyEncoding.Base64)
        {
            try
            {
                var keyParams = SM2KeyHelper.LoadPublicKey(publicKey, encoding);
                var engine = new SM2Engine();
                engine.Init(true, new ParametersWithRandom(keyParams, new SecureRandom()));
                byte[] cipherBytes = engine.ProcessBlock(data, 0, data.Length);
                // BC默认输出c1||c2||c3，转为国标c1||c3||c2
                cipherBytes = ChangeC1C2C3ToC1C3C2(cipherBytes);
                return Convert.ToBase64String(cipherBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2加密失败: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region 解密

        /// <summary>
        /// SM2私钥解密
        /// </summary>
        /// <param name="cipherText">Base64编码的密文</param>
        /// <param name="privateKey">私钥字符串</param>
        /// <param name="encoding">密钥编码格式</param>
        /// <returns>解密后的明文</returns>
        public static string Decrypt(string cipherText, string privateKey, SM2KeyEncoding encoding = SM2KeyEncoding.Base64)
        {
            try
            {
                var keyParams = SM2KeyHelper.LoadPrivateKey(privateKey, encoding);
                return Decrypt(cipherText, keyParams);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2解密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM2解密（使用密钥参数）
        /// </summary>
        public static string Decrypt(string cipherText, ECPrivateKeyParameters privateKey)
        {
            try
            {
                var engine = new SM2Engine();
                engine.Init(false, privateKey);
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                // 国标密文c1||c3||c2，转为BC格式c1||c2||c3再解密
                cipherBytes = ChangeC1C3C2ToC1C2C3(cipherBytes);
                byte[] plainBytes = engine.ProcessBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(plainBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2解密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM2解密为字节数组
        /// </summary>
        public static byte[] DecryptToBytes(string cipherText, string privateKey, SM2KeyEncoding encoding = SM2KeyEncoding.Base64)
        {
            try
            {
                var keyParams = SM2KeyHelper.LoadPrivateKey(privateKey, encoding);
                var engine = new SM2Engine();
                engine.Init(false, keyParams);
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                // 国标密文c1||c3||c2，转为BC格式c1||c2||c3再解密
                cipherBytes = ChangeC1C3C2ToC1C2C3(cipherBytes);
                return engine.ProcessBlock(cipherBytes, 0, cipherBytes.Length);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM2解密失败: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region 密文格式转换

        private const int C1_UNCOMPRESSED_LEN = 65; // 未压缩C1长度
        private const int C3_LEN = 32;              // SM3摘要长度

        /// <summary>
        /// BC输出c1||c2||c3 转为 国标c1||c3||c2
        /// </summary>
        private static byte[] ChangeC1C2C3ToC1C3C2(byte[] c1c2c3)
        {
            byte[] result = new byte[c1c2c3.Length];
            Buffer.BlockCopy(c1c2c3, 0, result, 0, C1_UNCOMPRESSED_LEN);                          // c1
            Buffer.BlockCopy(c1c2c3, c1c2c3.Length - C3_LEN, result, C1_UNCOMPRESSED_LEN, C3_LEN); // c3
            Buffer.BlockCopy(c1c2c3, C1_UNCOMPRESSED_LEN, result, C1_UNCOMPRESSED_LEN + C3_LEN, c1c2c3.Length - C1_UNCOMPRESSED_LEN - C3_LEN); // c2
            return result;
        }

        /// <summary>
        /// 国标c1||c3||c2 转为 BC格式c1||c2||c3
        /// </summary>
        private static byte[] ChangeC1C3C2ToC1C2C3(byte[] c1c3c2)
        {
            byte[] result = new byte[c1c3c2.Length];
            Buffer.BlockCopy(c1c3c2, 0, result, 0, C1_UNCOMPRESSED_LEN);                          // c1
            Buffer.BlockCopy(c1c3c2, C1_UNCOMPRESSED_LEN + C3_LEN, result, C1_UNCOMPRESSED_LEN, c1c3c2.Length - C1_UNCOMPRESSED_LEN - C3_LEN); // c2
            Buffer.BlockCopy(c1c3c2, C1_UNCOMPRESSED_LEN, result, c1c3c2.Length - C3_LEN, C3_LEN); // c3
            return result;
        }

        #endregion
    }

    /// <summary>
    /// SM4加解密工具类
    /// 支持随机密钥生成、十六进制输出
    /// </summary>
    public static class SM4CryptoHelper
    {
        /// <summary>
        /// SM4密钥长度（字节）
        /// </summary>
        public const int KEY_LENGTH = 16;

        /// <summary>
        /// SM4 IV长度（字节）
        /// </summary>
        public const int IV_LENGTH = 16;

        /// <summary>
        /// 固定IV（全零）
        /// </summary>
        public static readonly byte[] ZERO_IV = new byte[IV_LENGTH];

        #region 随机密钥生成

        /// <summary>
        /// 生成随机SM4密钥（16字节）
        /// </summary>
        /// <returns>16字节随机密钥</returns>
        public static byte[] GenerateRandomKey()
        {
            byte[] key = new byte[KEY_LENGTH];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }

        /// <summary>
        /// 生成随机SM4密钥（Base64编码）
        /// </summary>
        public static string GenerateRandomKeyBase64()
        {
            return Convert.ToBase64String(GenerateRandomKey());
        }

        /// <summary>
        /// 生成随机SM4密钥（十六进制编码）
        /// </summary>
        public static string GenerateRandomKeyHex()
        {
            return Hex.ToHexString(GenerateRandomKey());
        }

        #endregion

        #region CBC模式加密

        /// <summary>
        /// SM4-CBC加密（Base64输出）
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">密钥（16字节）</param>
        /// <param name="iv">IV向量（16字节，为null时使用全零IV）</param>
        /// <returns>Base64编码的密文</returns>
        public static string EncryptCBC(string plainText, byte[] key, byte[] iv = null)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText)) return plainText;
                ValidateKey(key);
                if (iv == null) iv = ZERO_IV;
                ValidateIV(iv);

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = EncryptCBC(plainBytes, key, iv);
                return Convert.ToBase64String(cipherBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM4-CBC加密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM4-CBC加密（十六进制输出）
        /// </summary>
        public static string EncryptCBCToHex(string plainText, byte[] key, byte[] iv = null)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText)) return plainText;
                ValidateKey(key);
                if (iv == null) iv = ZERO_IV;
                ValidateIV(iv);

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = EncryptCBC(plainBytes, key, iv);
                return SM2KeyHelper.BytesToHex(cipherBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM4-CBC加密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM4-CBC加密（字节数组）
        /// </summary>
        public static byte[] EncryptCBC(byte[] plainBytes, byte[] key, byte[] iv)
        {
            ValidateKey(key);
            if (iv == null) iv = ZERO_IV;
            ValidateIV(iv);

            var engine = new SM4Engine();
            var cbcBlockCipher = new CbcBlockCipher(engine);
            var pkcs7Padding = new Pkcs7Padding();
            var cipher = new PaddedBufferedBlockCipher(cbcBlockCipher, pkcs7Padding);

            var keyParam = new KeyParameter(key);
            var parameters = new ParametersWithIV(keyParam, iv);

            cipher.Init(true, parameters);

            int outputSize = cipher.GetOutputSize(plainBytes.Length);
            byte[] cipherBytes = new byte[outputSize];
            int length = cipher.ProcessBytes(plainBytes, 0, plainBytes.Length, cipherBytes, 0);
            length += cipher.DoFinal(cipherBytes, length);

            // 截取实际长度
            byte[] result = new byte[length];
            Array.Copy(cipherBytes, result, length);
            return result;
        }

        #endregion

        #region CBC模式解密

        /// <summary>
        /// SM4-CBC解密（Base64输入）
        /// </summary>
        public static string DecryptCBC(string cipherText, byte[] key, byte[] iv = null)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText)) return cipherText;
                ValidateKey(key);
                if (iv == null) iv = ZERO_IV;
                ValidateIV(iv);

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] plainBytes = DecryptCBC(cipherBytes, key, iv);
                return Encoding.UTF8.GetString(plainBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM4-CBC解密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM4-CBC解密（十六进制输入）
        /// </summary>
        public static string DecryptCBCFromHex(string hexCipherText, byte[] key, byte[] iv = null)
        {
            try
            {
                if (string.IsNullOrEmpty(hexCipherText)) return hexCipherText;
                ValidateKey(key);
                if (iv == null) iv = ZERO_IV;
                ValidateIV(iv);

                byte[] cipherBytes = SM2KeyHelper.HexToBytes(hexCipherText);
                byte[] plainBytes = DecryptCBC(cipherBytes, key, iv);
                return Encoding.UTF8.GetString(plainBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM4-CBC解密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM4-CBC解密（字节数组）
        /// </summary>
        public static byte[] DecryptCBC(byte[] cipherBytes, byte[] key, byte[] iv)
        {
            ValidateKey(key);
            if (iv == null) iv = ZERO_IV;
            ValidateIV(iv);

            var engine = new SM4Engine();
            var cbcBlockCipher = new CbcBlockCipher(engine);
            var pkcs7Padding = new Pkcs7Padding();
            var cipher = new PaddedBufferedBlockCipher(cbcBlockCipher, pkcs7Padding);

            var keyParam = new KeyParameter(key);
            var parameters = new ParametersWithIV(keyParam, iv);

            cipher.Init(false, parameters);

            int outputSize = cipher.GetOutputSize(cipherBytes.Length);
            byte[] plainBytes = new byte[outputSize];
            int length = cipher.ProcessBytes(cipherBytes, 0, cipherBytes.Length, plainBytes, 0);
            length += cipher.DoFinal(plainBytes, length);

            // 截取实际长度
            byte[] result = new byte[length];
            Array.Copy(plainBytes, result, length);
            return result;
        }

        #endregion

        #region ECB模式加密

        /// <summary>
        /// SM4-ECB加密（Base64输出）
        /// </summary>
        public static string EncryptECB(string plainText, byte[] key)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText)) return plainText;
                ValidateKey(key);

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = EncryptECB(plainBytes, key);
                return Convert.ToBase64String(cipherBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM4-ECB加密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM4-ECB加密（十六进制输出）
        /// </summary>
        public static string EncryptECBToHex(string plainText, byte[] key)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText)) return plainText;
                ValidateKey(key);

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = EncryptECB(plainBytes, key);
                return SM2KeyHelper.BytesToHex(cipherBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM4-ECB加密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM4-ECB加密（字节数组）
        /// </summary>
        public static byte[] EncryptECB(byte[] plainBytes, byte[] key)
        {
            ValidateKey(key);

            var engine = new SM4Engine();
            var ecbBlockCipher = new EcbBlockCipher(engine);
            var pkcs7Padding = new Pkcs7Padding();
            var cipher = new PaddedBufferedBlockCipher(ecbBlockCipher, pkcs7Padding);

            var keyParam = new KeyParameter(key);
            cipher.Init(true, keyParam);

            int outputSize = cipher.GetOutputSize(plainBytes.Length);
            byte[] cipherBytes = new byte[outputSize];
            int length = cipher.ProcessBytes(plainBytes, 0, plainBytes.Length, cipherBytes, 0);
            length += cipher.DoFinal(cipherBytes, length);

            byte[] result = new byte[length];
            Array.Copy(cipherBytes, result, length);
            return result;
        }

        #endregion

        #region ECB模式解密

        /// <summary>
        /// SM4-ECB解密（Base64输入）
        /// </summary>
        public static string DecryptECB(string cipherText, byte[] key)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText)) return cipherText;
                ValidateKey(key);

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] plainBytes = DecryptECB(cipherBytes, key);
                return Encoding.UTF8.GetString(plainBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM4-ECB解密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM4-ECB解密（十六进制输入）
        /// </summary>
        public static string DecryptECBFromHex(string hexCipherText, byte[] key)
        {
            try
            {
                if (string.IsNullOrEmpty(hexCipherText)) return hexCipherText;
                ValidateKey(key);

                byte[] cipherBytes = SM2KeyHelper.HexToBytes(hexCipherText);
                byte[] plainBytes = DecryptECB(cipherBytes, key);
                return Encoding.UTF8.GetString(plainBytes);
            } catch (Exception ex)
            {
                //LoggerHelper.Error($"SM4-ECB解密失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// SM4-ECB解密（字节数组）
        /// </summary>
        public static byte[] DecryptECB(byte[] cipherBytes, byte[] key)
        {
            ValidateKey(key);

            var engine = new SM4Engine();
            var ecbBlockCipher = new EcbBlockCipher(engine);
            var pkcs7Padding = new Pkcs7Padding();
            var cipher = new PaddedBufferedBlockCipher(ecbBlockCipher, pkcs7Padding);

            var keyParam = new KeyParameter(key);
            cipher.Init(false, keyParam);

            int outputSize = cipher.GetOutputSize(cipherBytes.Length);
            byte[] plainBytes = new byte[outputSize];
            int length = cipher.ProcessBytes(cipherBytes, 0, cipherBytes.Length, plainBytes, 0);
            length += cipher.DoFinal(plainBytes, length);

            byte[] result = new byte[length];
            Array.Copy(plainBytes, result, length);
            return result;
        }

        #endregion

        #region 验证方法

        /// <summary>
        /// 验证密钥长度
        /// </summary>
        private static void ValidateKey(byte[] key)
        {
            if (key == null || key.Length != KEY_LENGTH)
            {
                throw new ArgumentException($"SM4密钥长度必须为{KEY_LENGTH}字节，当前为{(key?.Length ?? 0)}字节");
            }
        }

        /// <summary>
        /// 验证IV长度
        /// </summary>
        private static void ValidateIV(byte[] iv)
        {
            if (iv == null || iv.Length != IV_LENGTH)
            {
                throw new ArgumentException($"SM4 IV长度必须为{IV_LENGTH}字节，当前为{(iv?.Length ?? 0)}字节");
            }
        }

        /// <summary>
        /// 解码密钥字符串为字节数组
        /// </summary>
        /// <param name="keyStr">密钥字符串</param>
        /// <param name="isHex">是否为十六进制格式</param>
        /// <returns>密钥字节数组</returns>
        public static byte[] DecodeKey(string keyStr, bool isHex = false)
        {
            if (isHex)
            {
                return SM2KeyHelper.HexToBytes(keyStr);
            } else
            {
                return Encoding.UTF8.GetBytes(keyStr);
            }
        }

        #endregion
    }

    /// <summary>
    /// SM2密钥编码格式
    /// </summary>
    public enum SM2KeyEncoding
    {
        /// <summary>Base64编码</summary>
        Base64 = 0,
        /// <summary>十六进制编码</summary>
        Hex = 1,
        /// <summary>PEM格式</summary>
        PEM = 2
    }

    /// <summary>
    /// SM2密钥加载/解码工具类
    /// </summary>
    public static class SM2KeyHelper
    {
        /// <summary>
        /// 加载公钥
        /// </summary>
        /// <param name="publicKey">公钥字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>公钥参数</returns>
        public static ECPublicKeyParameters LoadPublicKey(string publicKey, SM2KeyEncoding encoding)
        {
            switch (encoding)
            {
                case SM2KeyEncoding.PEM:
                    return LoadPublicKeyFromPem(publicKey);
                case SM2KeyEncoding.Hex:
                    return LoadPublicKeyFromHex(publicKey);
                case SM2KeyEncoding.Base64:
                default:
                    return LoadPublicKeyFromBase64(publicKey);
            }
        }

        /// <summary>
        /// 加载私钥
        /// </summary>
        /// <param name="privateKey">私钥字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>私钥参数</returns>
        public static ECPrivateKeyParameters LoadPrivateKey(string privateKey, SM2KeyEncoding encoding)
        {
            switch (encoding)
            {
                case SM2KeyEncoding.PEM:
                    return LoadPrivateKeyFromPem(privateKey);
                case SM2KeyEncoding.Hex:
                    return LoadPrivateKeyFromHex(privateKey);
                case SM2KeyEncoding.Base64:
                default:
                    return LoadPrivateKeyFromBase64(privateKey);
            }
        }

        /// <summary>
        /// 从PEM格式加载公钥
        /// </summary>
        private static ECPublicKeyParameters LoadPublicKeyFromPem(string pem)
        {
            try
            {
                using (var reader = new StringReader(pem))
                {
                    var pemReader = new PemReader(reader);
                    var obj = pemReader.ReadObject();

                    if (obj is ECPublicKeyParameters publicKey)
                        return publicKey;

                    if (obj is AsymmetricCipherKeyPair keyPair)
                        return (ECPublicKeyParameters)keyPair.Public;

                    throw new ArgumentException("PEM文件中未找到有效的公钥");
                }
            } catch (Exception ex)
            {
                throw new ArgumentException($"加载PEM公钥失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从PEM格式加载私钥
        /// </summary>
        private static ECPrivateKeyParameters LoadPrivateKeyFromPem(string pem)
        {
            try
            {
                using (var reader = new StringReader(pem))
                {
                    var pemReader = new PemReader(reader);
                    var obj = pemReader.ReadObject();

                    if (obj is ECPrivateKeyParameters privateKey)
                        return privateKey;

                    if (obj is AsymmetricCipherKeyPair keyPair)
                        return (ECPrivateKeyParameters)keyPair.Private;

                    throw new ArgumentException("PEM文件中未找到有效的私钥");
                }
            } catch (Exception ex)
            {
                throw new ArgumentException($"加载PEM私钥失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从Base64格式加载公钥
        /// </summary>
        private static ECPublicKeyParameters LoadPublicKeyFromBase64(string base64Key)
        {
            try
            {
                byte[] keyBytes = Convert.FromBase64String(base64Key);
                return DecodePublicKey(keyBytes);
            } catch (Exception ex)
            {
                throw new ArgumentException($"加载Base64公钥失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从Base64格式加载私钥
        /// </summary>
        private static ECPrivateKeyParameters LoadPrivateKeyFromBase64(string base64Key)
        {
            try
            {
                byte[] keyBytes = Convert.FromBase64String(base64Key);
                return DecodePrivateKey(keyBytes);
            } catch (Exception ex)
            {
                throw new ArgumentException($"加载Base64私钥失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从Hex格式加载公钥
        /// </summary>
        private static ECPublicKeyParameters LoadPublicKeyFromHex(string hexKey)
        {
            try
            {
                byte[] keyBytes = HexToBytes(hexKey);
                return DecodePublicKey(keyBytes);
            } catch (Exception ex)
            {
                throw new ArgumentException($"加载Hex公钥失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从Hex格式加载私钥
        /// </summary>
        private static ECPrivateKeyParameters LoadPrivateKeyFromHex(string hexKey)
        {
            try
            {
                byte[] keyBytes = HexToBytes(hexKey);
                return DecodePrivateKey(keyBytes);
            } catch (Exception ex)
            {
                throw new ArgumentException($"加载Hex私钥失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 解码公钥字节数组
        /// </summary>
        private static ECPublicKeyParameters DecodePublicKey(byte[] keyBytes)
        {
            var curve = GMNamedCurves.GetByName("sm2p256v1");
            var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            // 如果是65字节（04||X||Y），去掉前缀
            if (keyBytes.Length == 65 && keyBytes[0] == 0x04)
            {
                byte[] uncompressed = new byte[64];
                Array.Copy(keyBytes, 1, uncompressed, 0, 64);
                keyBytes = uncompressed;
            }

            // 64字节（X||Y）
            if (keyBytes.Length != 64)
            {
                throw new ArgumentException($"公钥长度必须为64字节(X||Y)或65字节(04||X||Y)，当前为{keyBytes.Length}字节");
            }

            var point = curve.Curve.DecodePoint(new byte[] { 0x04 }.Combine(keyBytes));
            return new ECPublicKeyParameters(point, domainParams);
        }

        /// <summary>
        /// 解码私钥字节数组
        /// </summary>
        private static ECPrivateKeyParameters DecodePrivateKey(byte[] keyBytes)
        {
            var curve = GMNamedCurves.GetByName("sm2p256v1");
            var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            // 32字节（D）
            if (keyBytes.Length != 32)
            {
                throw new ArgumentException($"私钥长度必须为32字节，当前为{keyBytes.Length}字节");
            }

            var d = new Org.BouncyCastle.Math.BigInteger(1, keyBytes);
            return new ECPrivateKeyParameters(d, domainParams);
        }

        /// <summary>
        /// 十六进制字符串转字节数组
        /// </summary>
        public static byte[] HexToBytes(string hex)
        {
            hex = hex.Replace(" ", "").Replace("\n", "").Replace("\r", "");

            if (hex.Length % 2 != 0)
                throw new ArgumentException("十六进制字符串长度必须为偶数");

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 字节数组转十六进制字符串
        /// </summary>
        public static string BytesToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToUpper();
        }

        /// <summary>
        /// 合并字节数组
        /// </summary>
        public static byte[] Combine(this byte[] first, byte[] second)
        {
            byte[] result = new byte[first.Length + second.Length];
            Array.Copy(first, 0, result, 0, first.Length);
            Array.Copy(second, 0, result, first.Length, second.Length);
            return result;
        }

        /// <summary>
        /// 生成SM2密钥对
        /// </summary>
        /// <param name="encoding">输出编码格式</param>
        /// <returns>密钥对（公钥, 私钥）</returns>
        public static (string publicKey, string privateKey) GenerateKeyPair(SM2KeyEncoding encoding = SM2KeyEncoding.Base64)
        {
            var curve = GMNamedCurves.GetByName("sm2p256v1");
            var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator("EC");
            keyPairGenerator.Init(new ECKeyGenerationParameters(domainParams, new SecureRandom()));

            var keyPair = keyPairGenerator.GenerateKeyPair();
            var publicKey = (ECPublicKeyParameters)keyPair.Public;
            var privateKey = (ECPrivateKeyParameters)keyPair.Private;

            // 提取公钥（去掉04前缀）
            byte[] pubKeyBytes = publicKey.Q.GetEncoded();
            byte[] pubKeyRaw = new byte[64];
            Array.Copy(pubKeyBytes, 1, pubKeyRaw, 0, 64);

            // 提取私钥
            byte[] privKeyBytes = privateKey.D.ToByteArrayUnsigned();

            switch (encoding)
            {
                case SM2KeyEncoding.Hex:
                    return (BytesToHex(pubKeyRaw), BytesToHex(privKeyBytes));
                case SM2KeyEncoding.PEM:
                    return (ExportPublicKeyToPem(publicKey), ExportPrivateKeyToPem(privateKey));
                case SM2KeyEncoding.Base64:
                default:
                    return (Convert.ToBase64String(pubKeyRaw), Convert.ToBase64String(privKeyBytes));
            }
        }

        /// <summary>
        /// 导出公钥为PEM格式
        /// </summary>
        private static string ExportPublicKeyToPem(ECPublicKeyParameters publicKey)
        {
            using (var writer = new StringWriter())
            {
                var pemWriter = new PemWriter(writer);
                pemWriter.WriteObject(publicKey);
                pemWriter.Writer.Flush();
                return writer.ToString();
            }
        }

        /// <summary>
        /// 导出私钥为PEM格式
        /// </summary>
        private static string ExportPrivateKeyToPem(ECPrivateKeyParameters privateKey)
        {
            using (var writer = new StringWriter())
            {
                var pemWriter = new PemWriter(writer);
                pemWriter.WriteObject(privateKey);
                pemWriter.Writer.Flush();
                return writer.ToString();
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ComputeMD5Hash(string input)
        {
            // 创建一个MD5对象
            using (MD5 md5 = MD5.Create())
            {
                // 将输入字符串转换为字节数组并计算哈希
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 将哈希值转换为十六进制字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Post请求(同步)
        /// </summary>
        /// <param name="POSTURL">请求url</param>
        /// <param name="PostData">请求数据</param>
        /// <param name="authType">认证类型</param>
        /// <param name="secrect">密钥，若无认证，则不传</param>
        /// <param name="timeout">超时时间，单位：秒，默认10分钟</param>
        /// <param name="serviceMode">服务的调用模式，由应用程序决定</param>
        /// <param name="appId">服务接口数据维护所在的系统，未指定时缺省为当前应用系统</param>
        /// <param name="timestamp">时间戳，示例：”1697162624390”</param>
        /// <param name="CheckResponse">服务响应后执行的委托方法，检查结果无效时记录错误日志并由调用方抛出异常</param>
        public static string HttpPost(string POSTURL, string PostData, AuthType authType, HipHttpHeader httpHeader,
                out Dictionary<string, string> responseHeaders, int timeout = 10 * 60)
        {
            HttpWebRequest myHttpWebRequest = null;
            HttpWebResponse response = null;

            DateTime beginTime = DateTime.Now;
            string result = string.Empty;

            try
            {
                //发送请求的数据
                if (POSTURL.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(CheckValidationResult);
                    myHttpWebRequest = WebRequest.Create(POSTURL) as HttpWebRequest;
                    myHttpWebRequest.ProtocolVersion = HttpVersion.Version10;
                } else
                {
                    myHttpWebRequest = WebRequest.Create(POSTURL) as HttpWebRequest;
                }

                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Timeout = timeout * 1000;

                if (authType == AuthType.Basic)
                {
                    WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
                    webHeaderCollection.Add("Authorization", "Basic " + httpHeader.Authorization);
                    myHttpWebRequest.Headers = webHeaderCollection;
                } else if (authType == AuthType.Oauth)    //医共体使用认证类型
                {
                    WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
                    webHeaderCollection.Add("Authorization", "Bearer " + httpHeader.Authorization);
                    webHeaderCollection.Add("appId", httpHeader.AppId);
                    webHeaderCollection.Add("timestamp", httpHeader.Timestamp);

                    webHeaderCollection.Add("version", httpHeader.Version);
                    webHeaderCollection.Add("enc-type", httpHeader.EncType);
                    webHeaderCollection.Add("sign-type", httpHeader.SignType);
                    webHeaderCollection.Add("encrypted-key", httpHeader.EncryptedKey);
                    webHeaderCollection.Add("sign-data", httpHeader.SignData);

                    myHttpWebRequest.Headers = webHeaderCollection;
                }

                UTF8Encoding encoding = new UTF8Encoding();
                if (PostData != null)
                {
                    byte[] byte1 = encoding.GetBytes(PostData);
                    myHttpWebRequest.ContentType = "application/json";//application/x-www-form-urlencoded
                    myHttpWebRequest.ContentLength = byte1.Length;
                    Stream newStream = myHttpWebRequest.GetRequestStream();
                    newStream.Write(byte1, 0, byte1.Length);
                    newStream.Close();
                }

                //发送成功后接收返回的信息
                response = (HttpWebResponse)myHttpWebRequest.GetResponse();

                // 将响应头信息添加到输出的JObject中    
                responseHeaders = new Dictionary<string, string>();
                foreach (string key in response.Headers.AllKeys)
                {
                    responseHeaders[key] = response.Headers[key];
                }

                using (Stream stream = response.GetResponseStream())
                {
                    Encoding enc = Encoding.GetEncoding("UTF-8");
                    using (StreamReader streamReader = new StreamReader(stream, enc))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }

                ////未出错但实际调用错误："不支持的路径..."为数据处理平台特有
                //bool isJson = true;//结构化了的返回结果不抛错
                //try { JObject.Parse(result); } catch { isJson = false; }
                //if (response.StatusCode >= HttpStatusCode.BadRequest
                //    || (!isJson && (result.IndexOf("不支持的路径") >= 0 || result.IndexOf("出错") >= 0 || result.IndexOf("失败") >= 0)))
                //{
                //    throw new Exception(result);
                //}

            } catch (WebException ex)
            {
                string strErrorMessage = ex.Message;

                //转换错误抛出
                var exResponse = ex.Response;
                if (exResponse != null)
                {
                    StreamReader streamReader = new StreamReader(exResponse.GetResponseStream(), Encoding.UTF8);
                    string strErrorResult = streamReader.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(strErrorResult))
                    {
                        try
                        {
                            //可能非Json无法解析，如400，405错误
                            JObject jObject = JObject.Parse(strErrorResult);
                            if (jObject["Msg"] != null)
                                strErrorMessage = jObject["Msg"].ToString();
                            else if (jObject["msg"] != null)
                                strErrorMessage = jObject["msg"].ToString();
                            else if (jObject["Message"] != null)
                                strErrorMessage = jObject["Message"].ToString();
                        } catch { }
                    }
                }

                throw new Exception(strErrorMessage, ex);
            } catch (Exception ex)
            {
                string strErrorMessage = ex.Message;
                throw new Exception(strErrorMessage, ex);
            } finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (myHttpWebRequest != null)
                {
                    myHttpWebRequest.Abort();
                }
            }

            return result;
        }

    #endregion
    }

    //-------------------------------实体----------------------------------
    #region 定义实体类
    /// <summary>
    /// 页面调用后台服务传入参数
    /// </summary>
    public class WebCallInput
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 应用id
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string appSecret { get; set; }
        /// <summary>
        /// 平台公钥
        /// </summary>
        public string platformPublicKey { get; set; }
        /// <summary>
        /// 客户端SM2公钥
        /// </summary>
        public string clientSM2PublicKey { get; set; }
        /// <summary>
        /// 客户端SM2私钥
        /// </summary>
        public string clientSM2PrivateKey { get; set; }
        /// <summary>
        /// 服务代码 各个具体接口代码
        /// </summary>
        public string serviceCode { get; set; }
        /// <summary>
        /// 业务数据
        /// </summary>
        public Object data { get; set; }
    }

    /// <summary>
    /// 接口统一请求明文体（加密前原始JSON）
    /// </summary>
    public class HipRequestRoot
    {
        public HipHead head { get; set; }
        public JObject body { get; set; }
    }

    public class HipHead
    {
        /// <summary>消息唯一流水UUID</summary>
        public string id { get; set; }
        /// <summary>请求时间 yyyy-MM-dd HH:mm:ss.fff</summary>
        public string request_timestamp { get; set; }
        /// <summary>调用系标识 如HIS/LIS</summary>
        public string app_code { get; set; }
        /// <summary>目标系统标识</summary>
        public string target_app_code { get; set; }
        /// <summary>服务代码 X_MPI_0001等</summary>
        public string service_code { get; set; }
    }

    public class HipHttpHeader
    {
        public string Authorization { get; set; }
        public string AppId { get; set; }
        public string Timestamp { get; set; } // yyyyMMddHHmmss
        public string Version { get; set; }
        public string EncType { get; set; } // 固定SM4
        public string SignType { get; set; } // 固定SM2
        public string EncryptedKey { get; set; } // Base64临时SM4密钥(平台公钥加密)
        public string SignData { get; set; } // SM2签名Base64
        //public string EncryptedData { get; set; } // HTTP Body十六进制密文
    }
    /// <summary>
    /// 外层根实体
    /// </summary>
    public class HipHttpResponse
    {
        public HipHttpResponseHead head { get; set; }
        public List<object> data { get; set; }
    }

    /// <summary>
    /// head节点信息
    /// </summary>
    public class HipHttpResponseHead
    {
        public string id { get; set; }
        public string target_id { get; set; }
        public int code { get; set; }
        public string timestamp { get; set; }
        public int count { get; set; }
        public string msg { get; set; }
    }
    #endregion

}
