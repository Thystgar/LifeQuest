import { useAccount } from '@/hooks/useAccount';
import React, { useState } from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Modal, TouchableWithoutFeedback } from 'react-native';

const AccountHeader: React.FC = () => {
    const { account } = useAccount();
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
                        <View style={styles.menuDrawer}>
                            <View style={styles.menuHeader}>
                                <View style={styles.menuHeaderRow}>
                                    <View style={styles.avatarLarge}>
                                        <Text style={styles.avatarInitials}>{initials}</Text>
                                    </View>
                                    <View>
                                        <Text style={styles.menuHeaderName}>{account.name || 'Account'}</Text>
                                    </View>
                                </View>
                            </View>
                            <View style={styles.menuList}>
                                <View style={styles.menuItem}>
                                    <Text style={styles.menuItemText}>Points: {account.points}</Text>
                                </View>
                                <View style={styles.menuItem}>
                                    <Text style={styles.menuItemText}>Group: Komarci</Text>
                                </View>
                                <View style={styles.menuSeparator} />
                                <TouchableOpacity style={styles.menuItem} onPress={() => { setMenuVisible(false); /* handle settings navigation here */ }}>
                                    <Text style={styles.menuItemText}>Settings</Text>
                                </TouchableOpacity>
                            </View>
                        </View>
                    </Modal>
                </>
            ) : null}
        </View>
    );
};

const styles = StyleSheet.create({
    menuSeparator: {
        height: 1,
        backgroundColor: '#eee',
        marginVertical: 4,
        marginHorizontal: 16,
    },
    menuHeader: {
        alignItems: 'flex-start',
        justifyContent: 'center',
        paddingTop: 32,
        paddingBottom: 16,
        borderBottomWidth: 1,
        borderBottomColor: '#eee',
        paddingHorizontal: 20,
    },
    menuHeaderRow: {
        flexDirection: 'row',
        alignItems: 'center',
    },
    menuHeaderName: {
        color: '#222',
        fontSize: 22,
        fontWeight: 'bold',
        marginLeft: 16,
    },
    menuHeaderPoints: {
        color: '#03A9F4',
        fontSize: 16,
        fontWeight: '600',
        marginLeft: 16,
        marginTop: 2,
    },
    headerContainer: {
        paddingVertical: 25,
        paddingHorizontal: 20,
        alignItems: 'center',
        flexDirection: 'row',
        justifyContent: 'flex-start',
    },
    avatarLarge: {
        width: 40,
        height: 40,
        borderRadius: 20,
        marginRight: 16,
        borderWidth: 2,
        borderColor: '#03A9F4',
        backgroundColor: '#B3E5FC',
        alignItems: 'center',
        justifyContent: 'center',
    },
    avatarInitials: {
        color: '#03A9F4',
        fontSize: 16,
        fontWeight: 'bold',
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
    menuDrawer: {
        position: 'absolute',
        top: 0,
        left: 0,
        bottom: 0,
        width: 300,
        height: '100%',
        backgroundColor: '#fff',
        shadowColor: '#000',
        shadowOffset: { width: 2, height: 0 },
        shadowOpacity: 0.2,
        shadowRadius: 4,
        elevation: 8,
        zIndex: 10,
        justifyContent: 'flex-start',
    },
    menuList: {
        flex: 1,
        justifyContent: 'flex-start',
        paddingTop: 40,
    },
    menuItem: {
        paddingVertical: 12,
        paddingHorizontal: 20,
    },
    menuItemText: {
        fontSize: 16,
        color: '#222',
    },
});

export default AccountHeader;
