import { useAccount } from '@/hooks/useAccount';
import { useAuth } from '@/hooks/useAuth';
import React, { useState } from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Modal, TouchableWithoutFeedback } from 'react-native';
import SettingsMenu from './SettingsMenu';

const AccountHeader: React.FC = () => {
    const { account } = useAccount();
    const { signOut } = useAuth();

    const [menuVisible, setMenuVisible] = useState(false);
    // Compute initials from account name
    const getInitials = (name: string | undefined) => {
        if (!name) return 'A';
        const parts = name.trim().split(' ');
        if (parts.length === 1) return parts[0][0].toUpperCase();
        return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
    };
    const initials = account ? getInitials(account.name) : null;

    return (
        <View style={styles.headerContainer}>
            {account ? (
                <>
                    <TouchableOpacity onPress={() => setMenuVisible(true)}>
                        <View style={styles.avatarLarge}>
                            <Text style={styles.avatarInitials}>{initials}</Text>
                        </View>
                        <View>
                        <Text style={styles.name}>{account.name || 'Account'}</Text>
                        </View>
                    </TouchableOpacity>
                    <View style={styles.infoContainer}>
                        <Text style={styles.points}>{account.points} points</Text>
                    </View>
                    <Modal
                        visible={menuVisible}
                        transparent
                        onRequestClose={() => setMenuVisible(false)}
                    >
                        <TouchableWithoutFeedback onPress={() => setMenuVisible(false)}>
                            <View style={styles.modalOverlay} />
                        </TouchableWithoutFeedback>
                        <SettingsMenu
                            visible={menuVisible}
                            initials={initials}
                            account={account}
                            onClose={() => setMenuVisible(false)}
                            onSignOut={signOut}
                            onSettings={() => { setMenuVisible(false); }}
                        />
                    </Modal>
                </>
            ) : null}
        </View>
    );
};

const styles = StyleSheet.create({
    headerContainer: {
        paddingVertical: 25,
        paddingHorizontal: 20,
        alignItems: 'center',
        flexDirection: 'row',
        justifyContent: 'flex-start',
    },
  name: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
    avatarLarge: {
        width: 48,
        height: 48,
        borderRadius: 24,
        backgroundColor: '#eee',
        alignItems: 'center',
        justifyContent: 'center',
        marginRight: 12,
    },
    avatarInitials: {
        fontSize: 20,
        fontWeight: 'bold',
        color: '#333',
    },
    infoContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'flex-end',
    },
    points: {
        fontSize: 22,
        fontWeight: '600',
    },
    modalOverlay: {
        position: 'absolute',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
    },
});

export default AccountHeader;
