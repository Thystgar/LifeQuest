import React, { useEffect, useState, useContext } from 'react';
import { View, Text, StyleSheet, Modal, TextInput, Button, TouchableOpacity, TouchableHighlight } from 'react-native';
import { SwipeListView } from 'react-native-swipe-list-view';
import { AccountContext } from '../App';
import { fetchRewards, redeemReward, addReward, deleteReward } from '../shared/api';
import { Reward, createReward } from '../shared/types';

export default function RewardsTab() {
  const account = useContext(AccountContext);

  const [rewardItems, setRewardItems] = useState<Reward[]>([]);
  const [modalVisible, setModalVisible] = useState(false);
  const [reward, setReward] = useState<Reward | null>(createReward());
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
      if (reward) {
        await addReward(reward);
      }
      await fetchData();
      setModalVisible(false);
    } catch (error) {
      console.error(error);
    }
  };

  const openModalForEdit = (reward: Reward) => {
    setReward(reward);
    setModalVisible(true);
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
          setReward(createReward());
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
              <Text style={styles.modalText}>{'Add New Reward'}</Text>
              <TextInput
                style={styles.input}
                placeholder="Name"
                value={reward?.name}
                onChangeText={name => setReward(prev => prev ? { ...prev, name } : null)}
              />
              <TextInput
                style={styles.input}
                placeholder="Description"
                value={reward?.description}
                onChangeText={description => setReward(prev => prev ? { ...prev, description: description } : null)}
                keyboardType="numeric"
              />
              <TextInput
                style={styles.input}
                placeholder="Value"
                value={reward?.value !== undefined && reward?.value !== 0 ? reward.value.toString() : ''}
                onChangeText={points => setReward(prev => prev ? { ...prev, value: parseInt(points)} : null)}
                keyboardType="numeric"
              />
              <View style={styles.buttonContainer}>
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
    alignItems: 'flex-end',
    paddingBottom: 0,
  },
  modalView: {
    width: '100%',
    minHeight: 300,
    maxHeight: '90%',
    backgroundColor: 'white',
    borderRadius: 0, // No rounded corners
    padding: 35,
    alignItems: 'flex-start', // Align items to the left
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: -2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 5,
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
  },
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
  modalOverlay: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
  },
});