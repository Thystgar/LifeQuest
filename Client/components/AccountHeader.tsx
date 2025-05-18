import React, { useContext } from 'react';
import { View, Image, Text, StyleSheet } from 'react-native';
import { AccountContext } from '../App';

const AccountHeader: React.FC = () => {
    const account = useContext(AccountContext);
    return (
        <View style={styles.headerContainer}>
            <Image source={require('../assets/images/account.png')} style={styles.avatarLarge} />
            <View style={styles.infoContainer}>
                <Text style={styles.name}>{account.name || 'Account'}</Text>
                <Text style={styles.points}>Points: {account.points}</Text>
            </View>
        </View>
    );
};

const styles = StyleSheet.create({
    headerContainer: {
        backgroundColor: '#E3F2FD',
        paddingVertical: 32,
        paddingHorizontal: 24,
        alignItems: 'flex-start',
        flexDirection: 'row',
        justifyContent: 'flex-start',
    },
    avatarLarge: {
        width: 56,
        height: 56,
        borderRadius: 28,
        marginRight: 20,
        borderWidth: 2,
        borderColor: '#03A9F4',
        backgroundColor: '#B3E5FC',
    },
    infoContainer: {
        flex: 0,
        justifyContent: 'center',
    },
    name: {
        color: '#222',
        fontSize: 22,
        fontWeight: 'bold',
        marginBottom: 4,
    },
    points: {
        color: '#03A9F4',
        fontSize: 16,
        fontWeight: '600',
    },
});

export default AccountHeader;
