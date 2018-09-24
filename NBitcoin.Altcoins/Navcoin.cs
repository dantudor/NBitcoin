using NBitcoin;
using NBitcoin.DataEncoders;
using NBitcoin.Protocol;
using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NBitcoin.Altcoins
{
	public class Navcoin : NetworkSetBase
	{
		public static Navcoin Instance { get; } = new Navcoin();

		public override string CryptoCode => "NAV";

		private Navcoin()
		{

		}
		//Format visual studio
		//{({.*?}), (.*?)}
		//Tuple.Create(new byte[]$1, $2)

		// @aguycalled Copied the first 6 from chainparamsseeds.h
		static Tuple<byte[], int>[] pnSeed6_main = {
      Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x65,0xa4,0x48,0xc3}, 44440),
      Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x6d,0x56,0x51,0xeb}, 44440),
      Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x6e,0xaf,0xf2,0xb4}, 44440),
      Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x6e,0x16,0xa7,0x23}, 44440),
      Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x76,0xb3,0xfb,0xd2}, 44440),
      Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x76,0x5c,0x0d,0x52}, 44440)
    };
		static Tuple<byte[], int>[] pnSeed6_test = {
      Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x2e,0x04,0x18,0x88}, 15556),
      Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb0,0x09,0x13,0xf5}, 15556),
    };

#pragma warning disable CS0618 // Type or member is obsolete
		public class NavcoinConsensusFactory : ConsensusFactory
		{
			private NavcoinConsensusFactory()
			{
			}

			public static NavcoinConsensusFactory Instance { get; } = new NavcoinConsensusFactory();

			public override BlockHeader CreateBlockHeader()
			{
				return new NavcoinBlockHeader();
			}
			public override Block CreateBlock()
			{
				return new NavcoinBlock(new NavcoinBlockHeader());
			}
		}

#pragma warning disable CS0618 // Type or member is obsolete
		public class AuxPow : IBitcoinSerializable
		{
			Transaction tx = new Transaction();

			public Transaction Transactions
			{
				get
				{
					return tx;
				}
				set
				{
					tx = value;
				}
			}

			uint nIndex = 0;

			public uint Index
			{
				get
				{
					return nIndex;
				}
				set
				{
					nIndex = value;
				}
			}

			uint256 hashBlock = new uint256();

			public uint256 HashBlock
			{
				get
				{
					return hashBlock;
				}
				set
				{
					hashBlock = value;
				}
			}

			List<uint256> vMerkelBranch = new List<uint256>();

			public List<uint256> MerkelBranch
			{
				get
				{
					return vMerkelBranch;
				}
				set
				{
					vMerkelBranch = value;
				}
			}

			List<uint256> vChainMerkleBranch = new List<uint256>();

			public List<uint256> ChainMerkleBranch
			{
				get
				{
					return vChainMerkleBranch;
				}
				set
				{
					vChainMerkleBranch = value;
				}
			}

			uint nChainIndex = 0;

			public uint ChainIndex
			{
				get
				{
					return nChainIndex;
				}
				set
				{
					nChainIndex = value;
				}
			}

			BlockHeader parentBlock = new BlockHeader();

			public BlockHeader ParentBlock
			{
				get
				{
					return parentBlock;
				}
				set
				{
					parentBlock = value;
				}
			}

			public void ReadWrite(BitcoinStream stream)
			{
				stream.ReadWrite(ref tx);
				stream.ReadWrite(ref hashBlock);
				stream.ReadWrite(ref vMerkelBranch);
				stream.ReadWrite(ref nIndex);
				stream.ReadWrite(ref vChainMerkleBranch);
				stream.ReadWrite(ref nChainIndex);
				stream.ReadWrite(ref parentBlock);
			}
		}

		public class NavcoinBlock : Block
		{
			public NavcoinBlock(NavcoinBlockHeader header) : base(header)
			{

			}

			public override ConsensusFactory GetConsensusFactory()
			{
				return NavcoinConsensusFactory.Instance;
			}
		}
		public class NavcoinBlockHeader : BlockHeader
		{
			const int VERSION_AUXPOW = (1 << 8);

			AuxPow auxPow = new AuxPow();

			public AuxPow AuxPow
			{
				get
				{
					return auxPow;
				}
				set
				{
					auxPow = value;
				}
			}

			// @aguycalled Not sure how POW hash needs to be defined for nav
			public override uint256 GetPoWHash()
			{
				var headerBytes = this.ToBytes();
				var h = NBitcoin.Crypto.SCrypt.ComputeDerivedKey(headerBytes, headerBytes, 1024, 1, 1, null, 32);
				return new uint256(h);
			}

			public override void ReadWrite(BitcoinStream stream)
			{
				base.ReadWrite(stream);
				if((Version & VERSION_AUXPOW) != 0)
				{
					if(!stream.Serializing)
					{
						stream.ReadWrite(ref auxPow);
					}
				}
			}
		}
#pragma warning restore CS0618 // Type or member is obsolete

		//Format visual studio
		//{({.*?}), (.*?)}
		//Tuple.Create(new byte[]$1, $2)
		//static Tuple<byte[], int>[] pnSeed6_main = null;
		//static Tuple<byte[], int>[] pnSeed6_test = null;		

		// @aguycalled Not sure how POW hash needs to be defined for nav
		static uint256 GetPoWHash(BlockHeader header)
		{
			var headerBytes = header.ToBytes();
			var h = NBitcoin.Crypto.SCrypt.ComputeDerivedKey(headerBytes, headerBytes, 1024, 1, 1, null, 32);
			return new uint256(h);
		}

		protected override void PostInit()
		{
			RegisterDefaultCookiePath("Navcoin");
		}

		protected override NetworkBuilder CreateMainnet()
		{
			NetworkBuilder builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000,
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000,
				BIP34Hash = new uint256("0xecb7444214d068028ec1fa4561662433452c1cbbd6b0f8eeb6452bcfa1d0a7d6"),
				// @aguycalled PowLimit = ~arith_uint256(0) >> 16
				PowLimit = new Target(new uint256("000001ffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = 30,
				PowTargetSpacing = 30,
				PowAllowMinDifficultyBlocks = false,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 15120, // 75% of 20160
				MinerConfirmationWindow = 20160,
				CoinbaseMaturity = 30,
				LitecoinWorkCalculation = true,
				ConsensusFactory = NavcoinConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 53 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 85 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 150 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("nav"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("nav"))
			// @aguycalled WHere do we find the magic?
			.SetMagic(0xcbc6680f)
			.SetPort(44440)
			.SetRPCPort(44444)
			.SetName("nav-main")
			.AddAlias("nav-mainnet")
			.AddAlias("navcoin-mainnet")
			.AddAlias("navcoin-main")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("nav.community", "seed.nav.community"),
				new DNSSeedData("navcoin.org", "seed.navcoin.org"),
			})
			.AddSeeds(ToSeed(pnSeed6_main))
			.SetGenesis("");
			return builder;
		}

		protected override NetworkBuilder CreateTestnet()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000,
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000,
				// @aguycalled PowLimit = ~arith_uint256(0) >> 16
				PowLimit = new Target(new uint256("00001fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = 30,
				PowTargetSpacing = 30,
				PowAllowMinDifficultyBlocks = false,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 300, // 75% of 400+
				MinerConfirmationWindow = 400,
				CoinbaseMaturity = 30,
				LitecoinWorkCalculation = true,
				ConsensusFactory = NavcoinConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 111 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 196 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x40, 0x88, 0x2B, 0xE1 })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x40, 0x88, 0xDA, 0x4E })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tnav"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tnav"))
			.SetMagic(0x92efc5a9)
			.SetPort(15556)
			.SetRPCPort(44445)
			.SetName("nav-test")
			.AddAlias("nav-testnet")
			.AddAlias("navcoin-test")
			.AddAlias("navcoin-testnet")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("nav.community", "seed.nav.community"),
				new DNSSeedData("navcoin.org", "seed.navcoin.org"),
			})
			.AddSeeds(ToSeed(pnSeed6_test))
			.SetGenesis("");
			return builder;
		}

		protected override NetworkBuilder CreateRegtest()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000,
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000,
				// @aguycalled PowLimit = ArithToUint256(~arith_uint256(0) >> 1);
				PowLimit = new Target(new uint256("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = 30,
				PowTargetSpacing = 30,
				PowAllowMinDifficultyBlocks = false,
				MinimumChainWork = uint256.Zero,
				PowNoRetargeting = true,
				RuleChangeActivationThreshold = 2700,
				MinerConfirmationWindow = 3600,
				CoinbaseMaturity = 100,
				LitecoinWorkCalculation = true,
				ConsensusFactory = NavcoinConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 111 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 196 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x40, 0x88, 0x2B, 0xE1 })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x40, 0x88, 0xDA, 0x4E })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tnav"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tnav"))
			.SetMagic(0x377b972d)
			.SetPort(18886)
			.SetRPCPort(44446)
			.SetName("nav-reg")
			.AddAlias("nav-regtest")
			.AddAlias("navcoin-reg")
			.AddAlias("navcoin-regtest")
			.SetGenesis("");
			return builder;
		}
	}
}
