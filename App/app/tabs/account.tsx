import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';

interface Account {
	name: string;
	points: number;
  }

export default function Account() {
  const [accountInfo, setAccountInfo] = useState<Account>({ name: '', points: 0 });
  
	useEffect(() => {
	  const fetchAccountInfo = async () => {
		try {
		  const response = await fetch('http://10.0.2.2:8080/account');
		  if (!response.ok) {
			throw new Error(`HTTP error! status: ${response.status}`);
		  }
		  const data = await response.json();
		  setAccountInfo(data);
		} catch (error) {
		  console.error('Error fetching account information:', error);
		}
	  };
	
	  fetchAccountInfo();
	}, []);
	
	return (
	  <View style={styles.container}>
		<Text>Name: {accountInfo.name}</Text>
		<Text>Points: {accountInfo.points} points</Text>
	  </View>
	);
}

const styles = StyleSheet.create({
	container: {
		flex: 1,
		backgroundColor: '#fff',
	},
});