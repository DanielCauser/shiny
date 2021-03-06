﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using Android.Bluetooth;
using Shiny.BluetoothLE.Peripherals;
using Android.Content;
using DroidGattStatus = Android.Bluetooth.GattStatus;


namespace Shiny.BluetoothLE
{
    public static class Extensions
    {
        public static IObservable<Unit> WhenAdapterStatusChanged(this IAndroidContext context)
            => context.WhenIntentReceived(BluetoothAdapter.ActionStateChanged).Select(_ => Unit.Default);

        public static IObservable<BluetoothDevice> WhenBondRequestReceived(this IAndroidContext context)
            => context.WhenDeviceEventReceived(BluetoothDevice.ActionPairingRequest);

        public static IObservable<BluetoothDevice> WhenBondStatusChanged(this IAndroidContext context)
            => context.WhenDeviceEventReceived(BluetoothDevice.ActionBondStateChanged);

        public static IObservable<BluetoothDevice> WhenDeviceNameChanged(this IAndroidContext context)
            => context.WhenDeviceEventReceived(BluetoothDevice.ActionNameChanged);

        public static IObservable<BluetoothDevice> WhenDeviceEventReceived(this IAndroidContext context, string action)
            => context.WhenIntentReceived(action).Select(intent =>
            {
                var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                return device;
            });

        public static BluetoothManager GetBluetooth(this IAndroidContext context)
            => (BluetoothManager)context.AppContext.GetSystemService(Context.BluetoothService);


        public static ConnectionState ToStatus(this ProfileState state)
        {
            switch (state)
            {
                case ProfileState.Connected:
                    return ConnectionState.Connected;

                case ProfileState.Connecting:
                    return ConnectionState.Connecting;

                case ProfileState.Disconnecting:
                    return ConnectionState.Disconnecting;

                case ProfileState.Disconnected:
                default:
                    return ConnectionState.Disconnected;
            }
        }


        public static DroidGattStatus ToNative(this GattState status)
            => (DroidGattStatus)Enum.Parse(typeof(DroidGattStatus), status.ToString());
    }
}