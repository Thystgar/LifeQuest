import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, Modal, TextInput, Button, TouchableOpacity, TouchableHighlight } from 'react-native';
import { SwipeListView } from 'react-native-swipe-list-view';

interface Reward {
  id: string;
  name: string;
  points: number;
  redeemed: boolean;
}

export default function Rewards() {
  
    const [rewardItems, setRewardItems] = useState<Reward[]>([]);
    const [modalVisible, setModalVisible] = useState(false);
    const [rewardName, setRewardName] = useState('');
    const [rewardPoints, setRewardPoints] = useState('');
    const [rewardId, setRewardId] = useState<string | null>(null);
  
    const fetchRewards = async () => {
      try {
        const response = await fetch('http://10.0.2.2:8080/rewards');
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        setRewardItems(data);
      } catch (error) {
        console.error('Error fetching rewards:', error);
      }
    };
  
    useEffect(() => {
      fetchRewards();
    }, []);
  
    const handleRewardRightSwipe = async (id: string) => {
      try {
        const response = await fetch(`http://10.0.2.2:8080/rewards/redeem?id=${id}`, {
          method: 'POST',
        });
  
        if (response.ok) {
          setRewardItems((prevItems) =>
            prevItems.map((item) =>
              item.id === id ? { ...item, redeemed: true } : item
            )
          );
        } else {
          console.error('Failed to complete reward');
        }
      } catch (error) {
        console.error('Error completing reward:', error);
      }
    };
  
    const handleRewardLeftSwipe = async (id: string) => {
      try {
        const response = await fetch(`http://10.0.2.2:8080/rewards/reactivate?id=${id}`, {
          method: 'POST',
        });
  
        if (response.ok) {
          setRewardItems((prevItems) =>
            prevItems.map((item) =>
              item.id === id ? { ...item, redeemed: false } : item
            )
          );
        } else {
          console.error('Failed to reactivate reward');
        }
      } catch (error) {
        console.error('Error reactivating reward:', error);
      }
    };
  
    const handleAddReward = async () => {
      try {
        const response = await fetch('http://10.0.2.2:8080/rewards', {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            Name: rewardName,
            Points: rewardPoints,
          }),
        });
  
        if (response.ok) {
          await fetchRewards();
          setModalVisible(false);
        } else {
          console.error(response);
        }
      } catch (error) {
        console.error('Error adding reward:', error);
      }
    };
  
    const handleDeleteReward = async (id: string) => {
      try {
        const response = await fetch(`http://10.0.2.2:8080/rewards?id=${id}`, {
          method: 'DELETE',
        });
  
        if (response.ok) {
          await fetchRewards();
          setModalVisible(false);
        } else {
          console.error('Failed to delete reward');
        }
      } catch (error) {
        console.error('Error deleting reward:', error);
      }
    };
  
    const handleUpdateReward = async () => {
      try {
        const response = await fetch('http://10.0.2.2:8080/rewards', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            Id: rewardId,
            Name: rewardName,
            Points: rewardPoints,
          }),
        });
  
        if (response.ok) {
          await fetchRewards();
          setModalVisible(false);
        } else {
          console.error(response);
          console.error('Failed to update reward');
        }
      } catch (error) {
        console.error('Error updating reward:', error);
      }
    };
  
    const openModalForEdit = (reward: Reward) => {
      resetModal();
      setRewardId(reward.id);
      setRewardName(reward.name);
      setRewardPoints(reward.points.toString());
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
                  {item.name} - {item.points} points
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
            } else {
              handleRewardLeftSwipe(rowKey);
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
          animationType="slide"
          transparent={true}
          visible={modalVisible}
          onRequestClose={() => {
            setModalVisible(!modalVisible);
          }}
        >
          <View style={styles.modalContainer}>
            <View style={styles.modalView}>
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
                <Button title="Cancel" onPress={() => setModalVisible(false)} />
                {rewardId && (
                  <Button title="Delete" color="red" onPress={() => handleDeleteReward(rewardId)} />
                )}
                <Button title="Submit" onPress={rewardId ? handleUpdateReward : handleAddReward} />
              </View>
            </View>
          </View>
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
      flex: 1,
      justifyContent: 'flex-end',
      alignItems: 'center',
      paddingBottom: 60, // Adjust this value based on the height of your bottom menu
    },
    modalView: {
      width: '100%',
      backgroundColor: 'white',
      borderTopLeftRadius: 20,
      borderTopRightRadius: 20,
      padding: 35,
      alignItems: 'center',
      shadowColor: '#000',
      shadowOffset: {
        width: 0,
        height: 2,
      },
      shadowOpacity: 0.25,
      shadowRadius: 4,
      elevation: 5,
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
      width: '80%',
      paddingHorizontal: 10,
    },
    buttonContainer: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      width: '80%',
    },
});