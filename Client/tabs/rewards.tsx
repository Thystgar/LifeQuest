import React, { useEffect, useState, useContext } from 'react';
import { View, Text, StyleSheet, Modal, TextInput, Button, TouchableOpacity, TouchableHighlight } from 'react-native';
import { SwipeListView } from 'react-native-swipe-list-view';
import { AccountContext } from '../App';
import { fetchRewards, redeemReward, addReward, deleteReward } from '../shared/api';
import { Reward } from '../shared/types';

export default function RewardsTab() {
  const account = useContext(AccountContext);

  const [rewardItems, setRewardItems] = useState<Reward[]>([]);
  const [modalVisible, setModalVisible] = useState(false);
  const [rewardName, setRewardName] = useState('');
  const [rewardPoints, setRewardPoints] = useState('');
  const [rewardId, setRewardId] = useState<string | null>(null);

  const fetchData = async () => {
    try {
      const data = await fetchRewards();
      setRewardItems(data);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleRewardRightSwipe = async (id: string) => {
    try {
      await redeemReward(id);
      setRewardItems((prevItems) =>
        prevItems.map((item) =>
          item.id === id ? { ...item, redeemed: true } : item
        )
      );
    } catch (error) {
      console.error(error);
    }
  };

  const handleAddReward = async () => {
    try {
      const newReward: Reward = {
        id: '',
        name: rewardName,
        value: parseInt(rewardPoints, 10),
        redeemed: false,
        description: ''
      };
      await addReward(newReward);
      await fetchData();
      setModalVisible(false);
    } catch (error) {
      console.error(error);
    }
  };

  const handleDeleteReward = async (id: string) => {
    try {
      await deleteReward(id);
      await fetchData();
      setModalVisible(false);
    } catch (error) {
      console.error(error);
    }
  };

  const openModalForEdit = (reward: Reward) => {
    resetModal();
    setRewardId(reward.id);
    setRewardName(reward.name);
    setRewardPoints(reward.value.toString());
    setModalVisible(true);
  };

  const resetModal = () => {
    setRewardId(null);
    setRewardName('');
    setRewardPoints('');
  };

  return (
    <View style={styles.container}>
      <SwipeListView
        data={rewardItems.sort((a, b) => Number(a.redeemed) - Number(b.redeemed))}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => (
          <TouchableHighlight
            onPress={() => openModalForEdit(item)}
            underlayColor={'#DDD'}
          >
            <View style={styles.rowFront}>
              <Text style={[styles.item, item.redeemed && styles.completedItem]}>
                {item.name} - {item.value} points
              </Text>
            </View>
          </TouchableHighlight>
        )}
        renderHiddenItem={({ item }) => (
          <View style={styles.rowBack}>
            <Text style={styles.backText}>Redeem</Text>
            <Text style={styles.backText}>Reactivate</Text>
          </View>
        )}
        leftOpenValue={75}
        rightOpenValue={-75}
        onRowOpen={(rowKey, rowMap, toValue) => {
          if (toValue > 0) {
            handleRewardRightSwipe(rowKey);
          }
          rowMap[rowKey].closeRow();
        }}
      />
      <TouchableOpacity
        style={styles.fab}
        onPress={() => {
          resetModal();
          setModalVisible(true);
        }}
      >
        <Text style={styles.fabText}>+</Text>
      </TouchableOpacity>
      <Modal
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => {
          setModalVisible(!modalVisible);
        }}
      >
        <TouchableOpacity
          style={styles.modalOverlay}
          activeOpacity={1}
          onPressOut={() => setModalVisible(false)}
        >
          <View style={styles.modalContainer}>
            <View style={styles.modalView}>
              <TouchableOpacity
                style={styles.closeButton}
                onPress={() => setModalVisible(false)}
              >
                <Text style={styles.closeButtonText}>×</Text>
              </TouchableOpacity>
              <Text style={styles.modalText}>{rewardId ? 'Edit Reward' : 'Add New Reward'}</Text>
              <TextInput
                style={styles.input}
                placeholder="Name"
                value={rewardName}
                onChangeText={setRewardName}
              />
              <TextInput
                style={styles.input}
                placeholder="Points"
                value={rewardPoints}
                onChangeText={setRewardPoints}
                keyboardType="numeric"
              />
              <View style={styles.buttonContainer}>
                {rewardId && (
                  <Button title="Delete" color="red" onPress={() => handleDeleteReward(rewardId)} />
                )}
                <Button title="Submit" onPress={ handleAddReward} />
              </View>
            </View>
          </View>
        </TouchableOpacity>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
  },
  rowFront: {
    backgroundColor: '#fff',
    borderBottomColor: '#ccc',
    borderBottomWidth: 1,
    padding: 10,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  item: {
    fontSize: 18,
  },
  completedItem: {
    textDecorationLine: 'line-through',
    color: 'green',
  },
  rowBack: {
    alignItems: 'center',
    backgroundColor: '#DDD',
    flex: 1,
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingLeft: 15,
  },
  backText: {
    color: '#FFF',
  },
  fab: {
    position: 'absolute',
    width: 56,
    height: 56,
    alignItems: 'center',
    justifyContent: 'center',
    right: 20,
    bottom: 20,
    backgroundColor: '#03A9F4',
    borderRadius: 30,
    elevation: 8,
  },
  fabText: {
    fontSize: 24,
    color: 'white',
  },
  modalContainer: {
    width: '100%',
    flex: 1,
    justifyContent: 'flex-end',
    alignItems: 'center',
    paddingBottom: 60, // Adjust this value based on the height of your bottom menu
  },
  modalView: {
    width: '100%',
    backgroundColor: 'white',
    borderRadius: 20,
    padding: 35,
    alignItems: 'flex-start', // Align items to the left
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 5,},
  modalText: {
    marginBottom: 15,
    textAlign: 'center',
    fontSize: 20,
    fontWeight: 'bold',
  },
  input: {
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    marginBottom: 15,
    width: '90%',
    paddingHorizontal: 10,
  },
  buttonContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: '90%',
  },
  closeButton: {
    position: 'absolute',
    top: 10,
    right: 10,
    zIndex: 1,
  },
  closeButtonText: {
    fontSize: 24,
    fontWeight: 'bold',
    color: 'black',
  },
  modalOverlay: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
  },
});