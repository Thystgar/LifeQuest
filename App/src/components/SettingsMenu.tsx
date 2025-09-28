import React from 'react';
import { View, Text, TouchableOpacity, Button, StyleSheet } from 'react-native';
import { useApi } from '@/hooks/useApi';
import { Group } from '@/api/models/Group';

interface SettingsMenuProps {
  visible: boolean;
  initials: string | null;
  account: { name?: string; points?: number } | null;
  onClose: () => void;
  onSignOut: () => void;
  onSettings: () => void;
}

const SettingsMenu: React.FC<SettingsMenuProps> = ({ visible, initials, account, onClose, onSignOut, onSettings }) => {
  const [group, setGroup] = React.useState<Group | null>(null);
  const {fetchGroup} = useApi();

  React.useEffect(() => {
    const loadGroup = async () => {
      const groupData = await fetchGroup();
      setGroup(groupData);
    };
    loadGroup();
  }, []);

  if (!visible || !account) return null;
  return (
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
          <Text style={styles.menuItemText}>Group: {group?.name || 'None'}</Text>
        </View>
        <View style={styles.menuSeparator} />
        <TouchableOpacity style={styles.menuItem} onPress={onSettings}>
          <Text style={styles.menuItemText}>Settings</Text>
        </TouchableOpacity>
        <View style={styles.menuItem}>
          <Button title="Sign out" onPress={onSignOut} />
        </View>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  menuDrawer: {
    position: 'absolute',
    left: 0,
    top: 0,
    height: '100%',
    width: 300,
    backgroundColor: '#fff',
    borderTopRightRadius: 16,
    borderBottomRightRadius: 16,
    elevation: 5,
    padding: 16,
    shadowColor: '#000',
    shadowOffset: { width: 2, height: 0 },
    shadowOpacity: 0.2,
    shadowRadius: 8,
  },
  menuHeader: {
    marginBottom: 12,
  },
  menuHeaderRow: {
    flexDirection: 'row',
    alignItems: 'center',
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
  menuHeaderName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  menuList: {
    marginTop: 8,
  },
  menuItem: {
    paddingVertical: 8,
    paddingHorizontal: 16,
  },
  menuItemText: {
    fontSize: 16,
    color: '#333',
  },
  menuSeparator: {
    height: 1,
    backgroundColor: '#eee',
    marginVertical: 4,
    marginHorizontal: 16,
  },
});

export default SettingsMenu;
