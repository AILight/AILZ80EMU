using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;

namespace AILZ80CPU.Operations
{
    public class OperationCALL : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead1 { get; set; }
        private Action<CPUZ80>? ExecuterForRead2 { get; set; }
        private Action<CPUZ80>? ExecuterForWrite1 { get; set; }
        private Action<CPUZ80>? ExecuterForWrite2 { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => { cpu.Address = cpu.Register.PC; cpu.Register.PC++; } },  // ジャンプ先アドレスの下位バイトをフェッチ
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead1 = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => {
                cpu.Register.Internal_16bit_Register_L = cpu.Bus.Data;
                cpu.Address = cpu.Register.PC;
                cpu.Register.PC++;
            } },  // ジャンプ先アドレスの上位バイトをフェッチ
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead2 = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => {
                cpu.Register.Internal_16bit_Register_H = cpu.Bus.Data;
                cpu.Register.Internal_16bit_Register = (ushort)((cpu.Register.Internal_16bit_Register_H << 8) | cpu.Register.Internal_16bit_Register_L);
            } },  // ジャンプ先アドレスを設定
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForWrite1 = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => {
                cpu.Register.SP--;
                cpu.Register.Internal_Memory_Pointer = cpu.Register.SP;
                cpu.Bus.Data = cpu.Register.PC_H;  // PCの上位バイト（PC_H）をスタックにプッシュ
            } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForWrite2 = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => {
                cpu.Register.SP--;
                cpu.Register.Internal_Memory_Pointer = cpu.Register.SP;
                cpu.Bus.Data = cpu.Register.PC_L;  // PCの下位バイト（PC_L）をスタックにプッシュ
                cpu.Register.PC = cpu.Register.Internal_16bit_Register;  // ジャンプ先アドレスにジャンプ
            } },
        };

        private OperationCALL(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationCALL Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.CALL)
            {
                return default!;
            }

            var executer = default(Action<CPUZ80>);
            var operationItem = new OperationCALL(instructionItem);

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
                if (operandExecuterForRead1.TryGetValue(instructionItem.Operand, out var executerForRead1))
                {
                    operationItem.ExecuterForRead1 = executerForRead1;
                }
                if (operandExecuterForRead2.TryGetValue(instructionItem.Operand, out var executerForRead2))
                {
                    operationItem.ExecuterForRead2 = executerForRead2;
                }
                if (operandExecuterForWrite1.TryGetValue(instructionItem.Operand, out var executerForWrite1))
                {
                    operationItem.ExecuterForWrite1 = executerForWrite1;
                }
                if (operandExecuterForWrite2.TryGetValue(instructionItem.Operand, out var executerForWrite2))
                {
                    operationItem.ExecuterForWrite2 = executerForWrite2;
                }

                return operationItem;
            }

            return default!;
        }

        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu); // フェッチサイクル
            }
            else if (cpu.TimingCycle == TimingCycleEnum.R1_T3_H)
            {
                if (machineCycleIndex == 1)
                {
                    ExecuterForRead1?.Invoke(cpu);  // メモリの読み取り1
                }
                else
                {
                    ExecuterForRead2?.Invoke(cpu);  // メモリの読み取り2
                }
            }
            else if (cpu.TimingCycle == TimingCycleEnum.W1_T3_H)
            {
                if (machineCycleIndex == 1)
                {
                    ExecuterForWrite1?.Invoke(cpu);  // スタックへの書き込み（上位バイト）
                }
                else
                {
                    ExecuterForWrite2?.Invoke(cpu);  // スタックへの書き込み（下位バイト）＆ジャンプ
                }
            }

            return this;
        }
    }
}
